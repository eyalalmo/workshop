
    var ws = new WebSocket("ws://" + window.location.host +"/api/WebSocket/");
    
                ws.onopen = function () {
        alert("connected");
    };
                ws.onmessage = function (evt) {
        alert(evt.data);
    };
                ws.onerror = function (evt) {
        alert(evt.data);
    };
                ws.onclose = function () {
        alert("disconnected");
    };
   

                $("#btnLogin").click(function () {
                if (ws.readyState == WebSocket.OPEN) {
        ws.send($("name5").val());
    }
                else {
        $("#spanStatus").text("Connection is closed");
    }
});