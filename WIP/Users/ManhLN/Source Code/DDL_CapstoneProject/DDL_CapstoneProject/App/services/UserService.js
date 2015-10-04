"use strict";

service.service('UserService', function ($http) {

    // Read all Accounts
    this.getAccounts = function () {
        return $http.get("/api/AccountApi/GetAllAccounts");
    }

    // Read Current Account
    this.getCurrentAccount = function () {
        return $http.get("/api/AccountApi/GetCurrentAccount");
    }

    // Check Login Status
    this.CheckLoginStatus = function ($q) {
        var deferred = $q.defer();
        var promiseGetProfile = $http.get("/api/AccountApi/CheckLoginStatus");
        promiseGetProfile.then(
                    function (result) {
                        if (result.data.Status === true && result.data.Message === "has_login") {
                            // Todo here.
                            deferred.resolve(result.data.Message);
                        } else {
                            deferred.reject();
                        }
                    },
                    function (error) {
                        deferred.reject();
                    });
        return deferred.promise;
    }

    //// Read Account by ID
    //this.getAccount = function (id) {
    //    return $http.get("/api/AccountApi/GetAccount/" + id);
    //}

    // Function to create new Account
    this.register = function (newUser) {
        var request = $http({
            method: 'post',
            url: '/api/UserApi/Register',
            data: newUser
        });

        return request;
    }

    // Function to edit a Account
    this.EditAccount = function (id, Account) {
        var request = $http({
            method: 'put',
            url: '/api/AccountApi/EditAccount/' + id,
            data: Account
        });

        return request;
    }

    // Function to edit a Account
    this.Login = function (User) {
        var request = $http({
            method: 'post',
            url: '/Account/Login/',
            data: User
        });

        return request;
    }
});