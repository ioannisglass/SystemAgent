import mysql.connector
from datetime import datetime

DATABASES = {
    'default': {
        'ENGINE': 'django.db.backends.mysql',
        'NAME': 'testdb',
        'USER': 'root',
        'PASSWORD': '123456',
        'HOST': '127.0.0.1',
        'PORT': '3306',
    }
}

class AgentManage():
    def __init__(self, db_host, db_port, db_user, db_pass, db_name):
        self.db_host = db_host
        self.db_port = db_port
        self.db_user = db_user
        self.db_pass = db_pass
        self.db_name = db_name
        
        self.my_db = mysql.connector.connect(
            host=self.db_host,
            user=self.db_user,
            port=self.db_port,
            passwd=self.db_pass,
            database=self.db_name
        )
        self.my_cursor = self.my_db.cursor()
        
    def signIn(self, userid, password):
        query = f"SELECT customerid FROM tbl_users WHERE userid = '{userid}' AND password = '{password}';"
        self.my_cursor.execute(query)
        ds = self.my_cursor.fetchall()
        ret = {}
        if ds == None or len(ds) == 0:
            ret["cusid"] = ""
            ret["status"] = "0"
            ret["message"] = "Invalid credential."
        else:
            ret["cusid"] = str(ds[0][0])
            ret["status"] = "2"
            ret["message"] = "Sign In Success"
        return ret
    
    def getActkeysByUserRowId(self, rowid):
        query = f"SELECT tbl_actkeys.*, COUNT(tbl_agents.id) as actkeycount FROM tbl_actkeys LEFT JOIN tbl_agents ON tbl_actkeys.id = tbl_agents.actkeyid WHERE tbl_actkeys.userid = {rowid} GROUP BY tbl_actkeys.id;"
        self.my_cursor.execute(query)
        ds = self.my_cursor.fetchall()
        ret = []
        if ds != None and len(ds) > 0:
            for row in ds:
                ret.append({
                    "title": row[6],
                    "actkey" : row[2],
                    "created" : row[4],
                    "status" : row[3],                 # enabled 2, disabled 1, deleted 0
                    "agents": row[7]
                })
        return ret
                
    def isActivated(self, cusid, actkey):
        query = f"SELECT tbl_users.*, tbl_actkeys.actkey FROM tbl_users LEFT JOIN tbl_actkeys " + \
            f"ON tbl_actkeys.userid = tbl_users.id WHERE tbl_users.customerid = '{cusid}' " + \
            f"AND tbl_actkeys.actkey = '{actkey}';"
        self.my_cursor.execute(query)
        ds = self.my_cursor.fetchall()
        if ds != None and len(ds) > 0:
            return "2"
        else:
            query = f"SELECT tbl_users.*, tbl_actkeys.actkey FROM tbl_users LEFT JOIN tbl_actkeys " + \
                f"ON tbl_actkeys.userid = tbl_users.id WHERE tbl_users.customerid = '{cusid}' " + \
                f"AND tbl_actkeys.actkey != '{actkey}';"
            self.my_cursor.execute(query)
            ds = self.my_cursor.fetchall()
            if ds != None and len(ds) > 0:
                return "1"
            else:
                return "0"
    
    # Store data of agent to db
    def saveAgentData(self, data):
        actkey = data["auth"]["actkey"]
        cusid = data["auth"]["cusid"]
        # After login, this function is executed, so do not need to check validation of actkey and cusid
        actkeyrid_sel_query = f"SELECT id FROM tbl_actkeys WHERE actkey = '{actkey}';"
        self.my_cursor.execute(actkeyrid_sel_query)
        ds = self.my_cursor.fetchall()
        if ds == None or len(ds) == 0:
            return False
        actkeyrid = ds[0][0]
        
        os = data["osInfo"]
        version = data["version"]
        host = data["machineName"]
        installedApps = data["installedApps"]
        action_at = datetime.now().strftime("%m/%d/%Y %H:%M")
        # check if the host is already existed
        query = f"SELECT * FROM tbl_agents WHERE `host` = '{host}' AND `actkeyid` = {actkeyrid};"
        self.my_cursor.execute(query)
        ds = self.my_cursor.fetchall()
        if ds == None or len(ds) == 0:
            # host is no existed, so it has to be added
            insert_query = f"INSERT INTO tbl_agents (actkeyid, host, os, version, created_at, updated_at) " + \
                f"VALUES ({actkeyrid}, '{host}', '{os}', '{version}', '{action_at}', '{action_at}');"
            self.my_cursor.execute(insert_query)
            self.my_db.commit()
            query = f"SELECT * FROM tbl_agents WHERE `host` = '{host}' AND `actkeyid` = {actkeyrid};"
            self.my_cursor.execute(query)
            ds = self.my_cursor.fetchall()
        # get agent row id with actkeyid and host
        agentrid = ds[0][0]
        # update the os and version with actkeyid and host
        update_query = f"UPDATE `tbl_agents` SET `os` = '{os}', `version` = '{version}' " + \
            f"WHERE `host` = '{host}' AND `actkeyid` = {actkeyrid};"
        self.my_cursor.execute(update_query)
        self.my_db.commit()
        # delete all apps to update with new list with agent row id
        delete_query = f"DELETE FROM tbl_installedapps WHERE `agentid` = {agentrid}"
        self.my_cursor.execute(delete_query)
        self.my_db.commit()
        # add the installed apps to tbl_installedapps
        query = f"INSERT INTO tbl_installedapps (name, version, agentid) VALUES "
        first_line = True
        for installed_app in installedApps:
            name = installed_app["displayName"]
            version = installed_app["displayVersion"]
            sub_query = f"('{name}', '{version}', '{agentrid}')"
            if first_line == False:
                query = query + ', '
            query = query + sub_query
            first_line = False
        query += ';'
        self.my_cursor.execute(query)
        self.my_db.commit()
        return True
    
    def getAgents(self, actkeyrowid, cusrowid):
        query = f"SELECT tbl_agents.* FROM tbl_agents LEFT JOIN tbl_actkeys " + \
            f"ON tbl_agents.actkeyid = tbl_actkeys.id LEFT JOIN tbl_users ON tbl_users.id = tbl_actkeys.userid " + \
            f"WHERE tbl_actkeys.id = {actkeyrowid} AND tbl_users.id = {cusrowid};"
        self.my_cursor.execute(query)
        ds = self.my_cursor.fetchall()
        ret = []
        if ds != None and len(ds) > 0:
            for row in ds:
                ret.append({
                    "id": row[0],
                    "os_info": row[3],
                    "com_name": row[2],
                    "version": row[4],
                    "updated_at": row[6]
                })
        return ret
    
    def getAgentApps(self, agentid):
        query = f"SELECT * FROM `tbl_installedapps` WHERE agentid = {agentid};"
        self.my_cursor.execute(query)
        ds = self.my_cursor.fetchall()
        ret = []
        if ds != None and len(ds) > 0:
            for row in ds:
                ret.append({
                    "name": row[2],
                    "version": row[3],
                })
        return ret