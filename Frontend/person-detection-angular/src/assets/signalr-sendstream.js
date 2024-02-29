const webm9MimeCodec = 'video/webm'
const videoSource = document.getElementById('video-stream-source')
const videoReceiver = document.getElementById('output-image')

const BufferStates = {
  Sending: 0,
  Receiving: 1
}

const maxBufferStreamCount = 10;
let bufferState = BufferStates.Sending

let bufferStreamIndex = 0;
let bufferStreamReceived = 0;

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

        console.log("Receiving: " + bufferStreamReceived)

        bufferStreamReceived++

        if (bufferStreamReceived === maxBufferStreamCount) {
            bufferStreamReceived = 0
            bufferStreamIndex = 0
            bufferState = BufferStates.Sending
        }
      }
      catch(error) {
        console.log(error)
      }
    })

    async function handleDataAvailable(event) {
        const ab64 = captureBase64Image();

        console.log("Sending: " + bufferStreamIndex)

        if (true) {
            subject.next(ab64);
            bufferStreamIndex++;
        }

        if (bufferStreamIndex == maxBufferStreamCount) {
            bufferState = BufferStates.Receiving
        }
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
