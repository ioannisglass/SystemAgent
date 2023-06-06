from flask import Flask, render_template, request

app = Flask(__name__)


@app.route('/ping')
def hello_world():
    return 'pong'

@app.route('/greet')
def greet():
    name = request.args.get('name', 'Guest')
    return f'Hello, {name}!'

@app.route('/auth', methods=['POST'])
def auth():
    data = request.get_json()
    cusid = data["cusid"]
    actkey = data["actkey"]
    if cusid == "testagent1" and actkey == "activationkey1":
        return "2"
    elif cusid == "testagent1" and actkey != "activationkey1":
        return "1"
    else:
        return "0"

@app.route('/submit', methods=['POST'])
def submit():
    data = request.get_json()
    # Process the submitted data
    return 'Data submitted successfully'

if __name__ == '__main__':
    app.run(host="localhost", port=5000, debug=True)