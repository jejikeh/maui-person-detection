from flask import Flask, request, jsonify
from flask_socketio import SocketIO

from imagegen import landmarker, object_detection

app = Flask(__name__)
socketio = SocketIO(
    app, debug=True, cors_allowed_origins='*')


@app.route("/imagegen", methods=["POST"])
def process_image():
    data = request.get_json()

    print(data)

    imageBase64 = data["content"]
    image = object_detection(imageBase64)
    image = landmarker(image)

    return {
        "content": image
    }


if __name__ == "__main__":
    socketio.run(app, port=12532, host="localhost")
