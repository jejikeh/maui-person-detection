from flask import Flask, request, jsonify

from imagegen import landmarker, object_detection

app = Flask(__name__)


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
    app.run(debug=True, host="localhost", port=12532)
