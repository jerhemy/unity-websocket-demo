// Importing the required modules
const WebSocketServer = require('ws');
 
// Creating a new websocket server
const wss = new WebSocketServer.Server({ port: 3000 })
const clients = new Map(); //keep track of clients

// Creating connection using websocket
wss.on("connection", (ws, req) => {
    ws.id = req.headers['sec-websocket-key'];
    clients.set(ws.id, ws);
    console.log(`new client connected, id: ${ws.id}`);

    // Spawn object for player
    // Send out spawn message to all clients
    let position = new PositionInfo(ws.id);
    let message = new Message();
    message.title = "spawn";
    message.content = position;
    let jsonMsg = JSON.stringify(message);
    clients.forEach(client => client.send(jsonMsg))

    // sending message
    ws.on("message", data => {
        
    });
    // handling what to do when clients disconnects from server
    ws.on("close", () => {
        console.log(`client has disconnected, id: ${ws.id}`);
        clients.delete(ws.id);
    });
    // handling client connection error
    ws.onerror = function () {
        console.log("Some Error occurred")
    }
});
console.log("The WebSocket server is running on port 3000");

// class for ws messages
class Message {
    title // string
    content // body of the message
}

// class for objects in Unity
class PositionInfo {
    owner // connection id that owns object
    position // vector3
    rotation // vector3
    constructor(id) {
        this.owner = id
    }
}