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

});