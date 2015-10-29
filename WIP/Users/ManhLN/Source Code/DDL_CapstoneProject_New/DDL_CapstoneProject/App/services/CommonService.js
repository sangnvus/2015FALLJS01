
service.service("CommmonService", function ($http, $window, $location, SweetAlert) {

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
                            $window.location.href = baseUrl + "login?returnUrl=" + $location.url();
                        } else if (result.data.Type === "not-found") {
                            $location.path("/notfound").replace();
                            deferred.reject();
                        } else {
                            $location.path("/error").replace();
                            deferred.reject();
                        }
                    },
                    function (error) {
                        $location.path("/error").replace();
                        deferred.reject();
                    });
        return deferred.promise;
    };

    this.checkError = function (result, baseUrl) {
        if (result === "not-authen") {
            $window.location.href = baseUrl + "login?returnUrl=" + $location.url();
            return false;
        } else if (result === "not-found") {
            SweetAlert.swal("Không tìm thấy", "Dữ liệu bạn truy cập không tồn tại", "warning");
            return false;
        } else if (result === "bad-request") {
            SweetAlert.swal("Đã có lỗi xảy ra", "Bạn hãy thử lại sau", "warning");
            return false;
        }
        return true;
    }

});