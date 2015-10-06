"use strict";

app.controller('MessageController', function ($scope, $location, conversations, MessageService, CommmonService) {
    //Todo here
    $scope.ListConversations = conversations.data.Data;

    function resetNewMessageModel() {
        $scope.NewMessage = {
            ToUser: "",
            Title: "",
            Content: ""
        }
    }

    $scope.Sent = false;

    resetNewMessageModel();

    $scope.searchFilter = function (obj) {
        var re = new RegExp($scope.searchText, 'i');
        return !$scope.searchText || re.test(obj.SenderName) || re.test(obj.ReceiverName) || re.test(obj.Title);
    };

    $scope.sendMessage = function() {
        var promisePost = MessageService.sendMessage($scope.NewMessage);

        promisePost.then(
            function (result) {
                if (result.data.Status === "success") {
                    $('#newMessageModel').modal('hide');
                    resetNewMessageModel();
                    if ($scope.Sent) {
                        $scope.ListConversations.unshift(result.data.Data);
                    }
                } else {
                    CommmonService.checkError(result.data.Type);
                    $scope.Error = result.data.Message;
                }
            },
            function (error) {
                $scope.Error = error.data.Message;
            });
    }

    function getListReceivedConversation() {
        var promiseGet = MessageService.getListReceivedConversations();

        promiseGet.then(
            function (result) {
                if (result.data.Status === "success") {
                    $scope.ListConversations = result.data.Data;
                } else {
                    CommmonService.checkError(result.data.Type);
                    $scope.Error = result.data.Message;
                }
            },
            function (error) {
                $scope.Error = error.data.Message;
            });
    }

    function getListSentConversation() {
        var promiseGet = MessageService.getListSentConversations();

        promiseGet.then(
            function (result) {
                if (result.data.Status === "success") {
                    $scope.ListConversations = result.data.Data;
                } else {
                    CommmonService.checkError(result.data.Type);
                    $scope.Error = result.data.Message;
                }
            },
            function (error) {
                $scope.Error = error.data.Message;
            });
    }

    $scope.changeInboxSent = function (value) {
        $scope.ListConversations = null;
        if (value === "inbox") {
            getListReceivedConversation();
            $scope.Sent = false;
        } else {
            getListSentConversation();
            $scope.Sent = true;
        }
    }

    $scope.showConversationDetail = function(id) {
        $location.path("/user/message/"+ id).replace();
    }
});