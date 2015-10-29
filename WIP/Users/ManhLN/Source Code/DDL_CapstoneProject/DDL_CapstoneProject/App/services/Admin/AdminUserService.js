﻿"use strict";

service.service("AdminUserService", function ($http) {
    this.getUserlist = function () {
        var request = $http({
            method: 'get',
            url: '../api/UserApi/GetUserListForAdmin'
        });
        return request;
    }

    this.changeUserStatus = function (username) {
        var request = $http({
            method: 'put',
            url: '../api/UserApi/ChangeUserStatus',
            params: {
                username: username
            }
        });
        return request;
    }

    this.getUserprofile = function (username) {
        var request = $http({
            method: 'get',
            url: '../api/UserApi/GetUserProfileForAdmin',
            params: {
                username: username
            }
        });
        return request;
    }
    
    this.getBackedProject = function (username) {
        var request = $http({
            method: 'get',
            url: '../api/UserApi/GetUserBackedProjectForAdmin',
            params: {
                username: username
            }
        });
        return request;
    }

    this.getCreatedProject = function (username) {
        var request = $http({
            method: 'get',
            url: '../api/UserApi/GetUserCreatedProjectForAdmin',
            params: {
                username: username
            }
        });
        return request;
    }

    this.getBackingDetail = function (username) {
        var request = $http({
            method: 'get',
            url: '../api/UserApi/GetUserBackingDetailForAdmin',
            params: {
                username: username
            }
        });
        return request;
    }

    this.getUserDasboard = function () {
        var request = $http({
            method: 'get',
            url: '../api/UserApi/GetUserDashboardForAdmin',
        });
        return request;
    }
    
    this.getListBacking = function () {
        var request = $http({
            method: 'get',
            url: '../api/UserApi/GetListBackingForAdmin',
        });
        return request;
    }

    this.getBackker = function (userName, backingID) {
        var request = $http({
            method: 'get',
            url: '../api/UserApi/GetBackerForAdmin',
            params: {
                userName: userName,
                backingID: backingID
            }
        });
        return request;
    }

});