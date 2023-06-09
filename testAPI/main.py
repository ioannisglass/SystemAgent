from flask import Flask, render_template, request
import json
from flask_cors import CORS, cross_origin
from MySQLHelper import AgentManage

agentManager = AgentManage(
    "localhost",
    3306,
    "root",
    "",
    "systemagent"
)
app = Flask(__name__)
CORS(app, support_credentials=True)

@app.route('/greet')
def greet():
    name = request.args.get('name', 'Guest')
    return f'Hello, {name}!'

# API to login with credentials and to get customer id and activation key
@app.route('/api/auth/signin', methods=['POST'])
@cross_origin(supports_credentials=True)
def auth():
    data = request.get_json()
    userid = data["userid"]
    password = data["password"]
    print(userid + ':' + password)
    ret = agentManager.signIn(userid, password)
    print(ret)
    return ret

# After successful log in, returns activation keys
@app.route('/api/actkeys', methods=['GET'])
@cross_origin(supports_credentials=True)
def get_act_keys():
    user_rowid = request.args.get('id')
    print(f'User row id: {user_rowid}')
    ret = agentManager.getActkeysByUserRowId(user_rowid)
    print(ret)
    return ret

# API to check activation with activation key and customer id
@app.route('/api/activate', methods=['POST'])
def activate():
    data = request.get_json()
    cusid = data["cusid"]
    actkey = data["actkey"]
    ret = agentManager.isActivated(cusid, actkey)
    print(ret)
    return ret

# API to submit device data(OS info, Machine Name, Third-Party applications, ...)
@app.route('/api/submit', methods=['POST'])
def submit():
    data = request.get_json()
    # Process the submitted data
    ret = agentManager.saveAgentData(data)
        
    response = {}
    if ret:
        response["status"] = "success"
        response["msg"] = 'Data submitted successfully'
    else:
        response["status"] = "failed"
        response["msg"] = 'Data submit failed'
    return response

@app.route('/api/agents')
def get_agents():
    cusidrowid = request.args.get('cusrid')
    actkeyrowid = request.args.get('actrid')
    machines = agentManager.getAgents(actkeyrowid, cusidrowid)
    print(machines)
    return machines

@app.route('/api/device')
def get_machine():
    agentid = request.args.get('id')
    installed_apps = agentManager.getAgentApps(agentid)
    return installed_apps

if __name__ == '__main__':
    app.run(host="localhost", port=5000, debug=True)