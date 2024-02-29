const webm9MimeCodec = 'video/webm'
const videoSource = document.getElementById('video-stream-source')
const videoReceiver = document.getElementById('output-image')

const connection = new signalR.HubConnectionBuilder()
    .withUrl("http://localhost:12532/video")
    .build();

let mediaRecorder = null
connection.start().then(() => {
    const subject = new signalR.Subject();
    connection.send("UploadClientData", subject);

    connection.on("SendPhoto", (data) => {
      try {
        videoReceiver.src = 'data:image/png;base64, ' + data
      }
      catch(error) {
        console.log(error)
      }
    })

    async function handleDataAvailable(event) {
        const ab64 = captureBase64Image();
        subject.next(ab64);
    }

    navigator.mediaDevices.getUserMedia({video: true, audio: false})
        .then(function (stream) {
            videoSource.srcObject = stream;

            videoSource.play();

            mediaRecorder = new MediaRecorder(stream, {mimeType: webm9MimeCodec});
            mediaRecorder.ondataavailable = handleDataAvailable
            mediaRecorder.start();

            setInterval(() => mediaRecorder.requestData(), 200)
        })
})

function captureBase64Image() {
    const canvas = document.createElement('canvas');
    canvas.width = 640;
    canvas.height = 640;

    canvas
      .getContext('2d')
      ?.drawImage(videoSource, 0, 0);

    return canvas
      .toDataURL('image/png')
      .replace(/^data:image\/(png|jpg);base64,/, '');
}
