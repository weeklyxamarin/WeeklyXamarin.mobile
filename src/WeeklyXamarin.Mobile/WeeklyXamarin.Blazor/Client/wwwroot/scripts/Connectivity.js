// Based on 
// https://www.patrickrobin.co.uk/articles/showing-connection-status-in-blazor-webassembly/
// https://www.neptuo.com/blog/2019/12/blazor-network-status/

let handler;

window.Network = {
    Initialize: function (interop) {
        console.log("initialize connectivity");
        handler = function () {
            console.log("handler" + navigator.onLine)
            interop.invokeMethodAsync("Network.StatusChanged", navigator.onLine);
        }

        window.addEventListener('online', handler);
        window.addEventListener('offline', handler);

        handler();
        
    },
    Dispose: function () {

        if (handler != null) {

            window.removeEventListener("online", handler);
            window.removeEventListener("offline", handler);
        }
    }
};
