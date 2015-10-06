﻿"use strict";

service.service('MessageService', function ($http) {

    // Function to create new conversation
    this.sendMessage = function (Message) {
        var request = $http({
            method: 'post',
            url: '/api/MessageApi/NewMessage',
            data: Message
        });

        return request;
    }

    // Function to get list messages
    this.getListReceivedConversations = function () {
        var request = $http({
            method: 'get',
            url: '/api/MessageApi/GetListReceivedConversations'
        });

        return request;
    }

    // Function to get list messages
    this.getListSentConversations = function () {
        var request = $http({
            method: 'get',
            url: '/api/MessageApi/GetListSentConversations'
        });

        return request;
    }

    this.getConversation = function (id) {
        var request = $http({
            method: 'get',
            url: '/api/MessageApi/GetConversation?id=' + id
        });

        return request;
    }

    this.Reply = function (reply) {
        var request = $http({
            method: 'post',
            url: '/api/MessageApi/Reply',
            data: reply
        });

        return request;
    }

});