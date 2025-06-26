// CommandManager.js

// from https://stackoverflow.com/questions/46154454/command-pattern-in-javascript-es6

class CommandManager {

    constructor() {
        this.executeHistory = [];
        this.undoHistory = [];
    }

    execute(command) {
        this.executeHistory.push(command);
        command.execute();
        //console.log(`Executed command ${command.serialize()}`);
    }

    undo() {
        let command = this.executeHistory.pop();
        if (command) {
            this.undoHistory.push(command);
            command.undo();
            //console.log(`Undo command ${command.serialize()}`)
        }
    }

    redo() {
        let command = this.undoHistory.pop();
        if (command) {
            this.executeHistory.push(command);
            command.execute();
            //console.log(`Redo command ${command.serialize()}`);
        }
    }
}

class Command {

    constructor(execute, undo, serialize, value) {
        this.execute = execute;
        this.undo = undo;
        this.serialize = serialize;
        this.value = value;
    }

}

// examples

//export function UpdateCommand(key, value) {

//    let oldValue;

//    const execute = () => {
//        if (mockupDB.hasOwnProperty(key)) {
//            oldValue = mockupDB[key];
//            mockupDB[key] = value;
//        }
//    };

//    const undo = () => {
//        if (oldValue) {
//            mockupDB[key] = oldValue;
//        }
//    };

//    const serialize = () => {
//        return JSON.stringify({ type: "Command", action: "update", key: key, value: value });
//    };

//    return new Command(execute, undo, serialize, value);
//}


//export function DeleteCommand(key) {

//    let oldValue;

//    const execute = () => {
//        if (mockupDB.hasOwnProperty(key)) {
//            oldValue = mockupDB[key];
//            delete mockupDB[key];
//        }
//    };

//    const undo = () => {
//        mockupDB[key] = oldValue;
//    };

//    const serialize = () => {
//        return JSON.stringify({ type: "Command", action: "delete", key: key });
//    };

//    return new Command(execute, undo, serialize);
//}