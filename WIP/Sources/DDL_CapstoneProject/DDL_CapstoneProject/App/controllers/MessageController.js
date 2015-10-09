"use strict";

app.controller('MessageController',
    function ($scope, $location, $rootScope, toastr, conversations, MessageService, CommmonService) {
    //Todo here
    $scope.ListConversations = conversations.data.Data;

    function resetNewMessageModel() {
        $scope.NewMessage = {
            ToUser: "",
            Title: "",
            Content: ""
        }
    }

    $scope.Unread = conversations.data.Data.length;

    $scope.selection = [];

    $scope.Sent = false;

    resetNewMessageModel();

    $scope.searchFilter = function (obj) {
        var re = new RegExp($scope.searchText, 'i');
        return !$scope.searchText || re.test(obj.SenderName) || re.test(obj.ReceiverName) || re.test(obj.Title);
    };

    $scope.ToggleSelection = function (id) {
        if ($scope.selection.indexOf(id) === -1) {
            $scope.selection.push(id);
        } else {
            $scope.selection.splice(id);
        }
    }

    $scope.showConversationDetail = function (id) {
        $location.path("/user/message/" + id).replace();
    }

    $scope.showConversationDetail = function (id) {
        $location.path("/user/message/" + id).replace();
    }

    // Function request 
    $scope.sendMessage = function () {
        if ($scope.NewMessage.Content.trim() !== "") {
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
                        toastr.error($scope.Error, 'Lỗi!');
                    }
                },
                function (error) {
                    $scope.Error = error.data.Message;
                });
        } else {
            toastr.warning("Bạn chưa nhập nội dung tin nhắn", 'Thông báo!');
        }
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
            $scope.selection = [];
        } else {
            getListSentConversation();
            $scope.Sent = true;
            $scope.selection = [];
        }
    }

    $scope.Delete = function () {
        var promise = MessageService.DeleteMessages($scope.selection);
        promise.then(
            function (result) {
                if (result.data.Status === "success") {
                    toastr.success('Đã xóa thành công!');
                    if ($scope.Sent === true) {
                        getListSentConversation();
                    } else {
                        getListReceivedConversation();
                    }
                    $scope.selection = [];
                } else {
                    CommmonService.checkError(result.data.Type);
                    $scope.Error = result.data.Message;
                    toastr.error($scope.Error, 'Lỗi!');
                }
            },
            function (error) {
                $scope.Error = error.data.Message;
                toastr.error($scope.Error, 'Lỗi!');
            });
    }

    $scope.getUserName = function (val) {
        var promise = UserService.getUserName(val);
        return promise.then(
            function (result) {
                if (result.data.Status === "success") {
                    return result.data.Data;
                }
            },
            function (error) {
                $scope.Error = error.data.Message;
                toastr.error($scope.Error, 'Lỗi!');
            });
    };
});