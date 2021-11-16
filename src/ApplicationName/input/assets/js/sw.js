self.addEventListener('install', function (e) {
    self.skipWaiting();
});

self.addEventListener('activate', function (e) {
    console.log(`Unregistering service worker`)
    self.registration.unregister()
        .then(function () {
            return self.clients.matchAll();
        })
        .then(function (clients) {
            clients.forEach(client => {
                console.log(`Navigating ${client.url}`)
                client.navigate(client.url)
            })
        });
});
