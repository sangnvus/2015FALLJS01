"use strict";

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

    this.Delete = function (id) {
        var request = $http({
            method: 'delete',
            url: '/api/MessageApi/DeleteMessage?id=' + id
        });

        return request;
    }

    this.DeleteMessages = function (listId) {
        var request = $http({
            method: 'delete',
            url: '/api/MessageApi/DeleteMessageList',
            params: {
                ids: listId
            }
        });

        return request;
    }

    this.getNumberNewMessage = function () {
        var request = $http({
            method: 'get',
            url: '/api/MessageApi/GetNumberNewMessage'
        });

        return request;
    }

});