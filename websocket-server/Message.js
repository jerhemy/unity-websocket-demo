function CreateSpawnMessage(id) {
  let position = new PositionInfo(id);
  let message = new Message();
  message.title = "spawn";
  message.content = position;
  return JSON.stringify(message);
}

// class for ws messages
class Message {
  title // string
  content // body of the message
  constructor(title, content) {
    this.title = title;
    this.content = content;
  }
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

module.exports = {CreateSpawnMessage, Message, PositionInfo};