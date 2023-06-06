from flask import Flask, render_template, request

app = Flask(__name__)


@app.route('/ping')
def hello_world():
    return 'pong'

@app.route('/greet')
def greet():
    name = request.args.get('name', 'Guest')
    return f'Hello, {name}!'

@app.route('/submit', methods=['POST'])
def submit():
    data = request.get_json()
    # Process the submitted data
    return 'Data submitted successfully'

if __name__ == '__main__':
    app.run(host="localhost", port=5000, debug=True)