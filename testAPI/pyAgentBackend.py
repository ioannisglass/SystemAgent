from flask import Flask, render_template, request
import json

app = Flask(__name__)

@app.route('/greet')
def greet():
    name = request.args.get('name', 'Guest')
    return f'Hello, {name}!'

# API to login with credentials and to get customer id and activation key
@app.route('/api/auth', methods=['POST'])
def auth():
    data = request.get_json()
    userid = data["userid"]
    password = data["password"]
    print(userid + ':' + password)
    response = data
    response["cusid"] = ""
    response["actkeys"] = []
    response["status"] = ""
    
    if userid == "testuser1" and password == "password1":
        response["cusid"] = "customer1"
        actKeys = []
        actKeys.append({
            "title": "Windows Desktop device",
            "actkey" : "activationkey1",
            "created" : "created1",
            "status" : "2",                 # enabled
            "agents": 3
            }
        )
        actKeys.append({
            "title": "Windows 2016 servers",
            "actkey" : "activationkey2",
            "created" : "created2",
            "status" : "1",                 # disabled
            "agents": 10
            }
        )
        actKeys.append({
            "title": "Linux devices",
            "actkey" : "activationkey3",
            "created" : "created3",
            "status" : "0",                 # deleted
            "agents": 20
            }
        )
        response["actkeys"] = actKeys
        response["status"] = "Sign In Success"
    elif userid == "testuser1" and password != "password1":
        response["status"] = "Password Wrong"
    else:
        response["status"] = "No User"
    return response

# API to check activation with activation key and customer id
@app.route('/api/activate', methods=['POST'])
def activate():
    data = request.get_json()
    cusid = data["cusid"]
    actkey = data["actkey"]
    if cusid == "customer1" and actkey == "activationkey1":
        return "2"      # Registered customer, Activated key
    elif cusid == "customer1" and actkey != "activationkey1":
        return "1"      # Registered customer, Un activated key
    else:
        return "0"      # Unregistered cutomer

# API to submit device data(OS info, Machine Name, Third-Party applications, ...)
@app.route('/api/submit', methods=['POST'])
def submit():
    data = request.get_json()
    # Process the submitted data
    #return 'Data submitted successfully'
    response = {}
    response["status"] = "success"
    response["msg"] = 'Data submitted successfully'
    return response

@app.route('/api/agents')
def get_agents():
    cusid = request.args.get('cusid')
    actkey = request.args.get('actkey')
    # Run Query items
    response = {}
    response["cusid"] = cusid
    response["actkey"] = actkey
    response["title"] = "Windows Desktop device"
    response["created"] = "created2"
    response["agents"] = 3
    response["status"] = "2"                 # enabled
    
    machines = []
    machines.append({
        "id": 123,
        "OS info": "Windows 11 64bit",
        "Computer Name": "DESKTOP-6X37DB"
    })
    machines.append({
        "id": 124,
        "OS info": "Windows 10 32bit",
        "Computer Name": "DESKTOP-5G37VS"
    })
    machines.append({
        "id": 125,
        "OS info": "Windows 11 64bit",
        "Computer Name": "DESKTOP-3H37S3"
    })
    response["machines"] = machines
    return response

@app.route('/api/device')
def get_machine():
    cusid = request.args.get('cusid')
    actkey = request.args.get('actkey')
    deviceid = request.args.get('id')
    response = {}
    response["cusid"] = cusid
    response["actkey"] = actkey
    response["deviceid"] = deviceid
    response["osinfo"] = "Windows 11 64bit"
    response["computername"] = "DESKTOP-3H37S3"
    
    installed_apps = []
    installed_apps.append({
        "name": "Postman",
        "version": "10.14.2"
    })
    installed_apps.append({
        "name": "Notepad++",
        "version": "8.5.3"
    })
    installed_apps.append({
        "name": "Ava",
        "version": "1.7.9.0"
    })
    response["installedapps"] = installed_apps
    return response

if __name__ == '__main__':
    app.run(host="localhost", port=5000, debug=True)