
service.service("CommmonService", function ($http, $window, $location) {

    //Read all Categories  
    this.checkHttpResult = function ($q, promise, baseUrl) {
        var deferred = $q.defer();
        promise.then(
                    function (result) {
                        if (result.data.Status === "success") {
                            // Todo here.
                            deferred.resolve(result);
                        } else if (result.data.Type === "not-authen") {
                            deferred.reject();
                            $window.location.href = baseUrl + "login";
                        } else if (result.data.Type === "not-found") {
                            $location.path("/notfound").replace();
                            deferred.reject();
                        } else {
                            deferred.reject();
                        }
                    },
                    function (error) {
                        deferred.reject();
                    });
        return deferred.promise;
    };

    this.checkError = function (result) {
        if (result === "not-authen") {
            $window.location.href = baseUrl + "login";
        } else if (result === "not-found") {
            //$location.path("/notfound").replace();
        } else {
            //$location.path("/error").replace();
        }
    }

});