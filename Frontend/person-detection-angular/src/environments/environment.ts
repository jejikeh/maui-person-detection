export const environment = {
  backend: 'http://localhost:12532',
  webrtcBackend: 'ws://localhost:12532/video',
  RTCPeerConfiguration: {
    iceServers: [
      {
        urls: 'stun:stun1.l.google.com:19302',
      },
    ],
  },
};
