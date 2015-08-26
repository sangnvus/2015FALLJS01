"use strict";

app.service('AccountService', function ($http) {

    // Read all Accounts
    this.getAccounts = function () {
        return $http.get("/api/AccountApi/GetAllAccounts");
    }

    // Read Account by ID
    this.getAccount = function (id) {
        return $http.get("/api/AccountApi/GetAccount/" + id);
    }

    // Function to create new Account
    this.post = function (Account) {
        var request = $http({
            method: 'post',
            url: '/api/AccountApi/CreateAccount',
            data: Account
        });

        return request;
    }

    // Function to edit a Account
    this.put = function (id, Account) {
        var request = $http({
            method: 'put',
            url: '/api/AccountApi/EditAccount/' + id,
            data: Account
        });

        return request;
    }
});