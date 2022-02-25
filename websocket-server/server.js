// Importing the required modules
const WebSocketServer = require('ws');
 
// Creating a new websocket server
const wss = new WebSocketServer.Server({ port: 3000 })
const clients = []; //keep track of clients

// Creating connection using websocket
wss.on("connection", (ws, req) => {
    ws.id = req.headers['sec-websocket-key'];
    clients[ws.id] = ws.id
    console.log(`new client connected, id: ${ws.id}`);
    // sending message
    ws.on("message", data => {
        console.log(`Client has sent us: ${data}`)
        ws.send(`Hello, ${data}`);
    });
    // handling what to do when clients disconnects from server
    ws.on("close", () => {
        console.log(`client has disconnected, id: ${ws.id}`);
        clients.splice(ws.id, 1); // remove connection from clients
    });
    // handling client connection error
    ws.onerror = function () {
        console.log("Some Error occurred")
    }
});
console.log("The WebSocket server is running on port 3000");