"use strict";

service.service('UserService', function ($http) {

    // Function to create new Account
    this.register = function (newUser) {
        var request = $http({
            method: 'post',
            url: '/api/UserApi/Register',
            data: newUser
        });

        return request;
    }

    // Function to reset password
    this.resetPassword = function (email) {
        var request = $http({
            method: 'post',
            url: '/api/UserApi/ResetPassword?email=' + email,
        });

        return request;
    }

    // Function to get list username filter by value
    this.getUserName = function (val) {
        var request = $http({
            method: 'get',
            url: '/api/UserApi/GetListUserName',
            params: {
                username: val
            }
        });

        return request;
    }

    // Function to check login status
    this.checkLoginStatus = function (val) {
        var request = $http({
            method: 'get',
            url: '/api/UserApi/CheckLoginStatus',
        });

        return request;
    }

});