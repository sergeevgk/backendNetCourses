function onMessageFunction(message) {
    alert(message);
    document.getElementById('message-table').innerHTML += "<tr><td>" + message + "</td></tr>";
    var onMessageEvent = new CustomEvent('onMessage', { message });
    document.dispatchEvent(onMessageEvent);
}
