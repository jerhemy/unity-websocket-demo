// Importing the required modules
const WebSocketServer = require('ws');
const {CreateSpawnMessage, Message, PositionInfo} = require("./Message");
 
// Creating a new websocket server
const wss = new WebSocketServer.Server({ port: 3000 })
const clients = new Map(); //keep track of clients

// Creating connection using websocket
wss.on("connection", (ws, req) => {
    // Tell new client about other players
    clients.forEach(client => {
        ws.send(CreateSpawnMessage(client.id));
    })

    // Add client to Map with id
    ws.id = req.headers['sec-websocket-key'];
    clients.set(ws.id, ws);
    console.log(`new client connected, id: ${ws.id}`);
    ws.send(JSON.stringify(new Message("client_id", new PositionInfo(ws.id)))); // Send id to client

    // Spawn object for player
    // Send out spawn message to all clients
    clients.forEach(client => client.send(CreateSpawnMessage(ws.id)))

    // sending message
    ws.on("message", data => {
        // console.log(JSON.parse(data));
        clients.forEach(client => client.send(data))
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