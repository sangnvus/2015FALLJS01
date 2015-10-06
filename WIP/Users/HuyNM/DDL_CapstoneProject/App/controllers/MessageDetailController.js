"use strict";

app.controller('MessageDetailController', function ($scope, $location, conversation, toastr, MessageService, CommmonService) {
    //Todo here.
    $scope.Conversation = conversation.data.Data;

    $scope.Reply = {
        ConversationID: $scope.Conversation.ConversationID,
        Content: ""
    }

    // function reply a conversation
    $scope.ReplyMessage = function () {
        if ($scope.Reply.Content.trim() !== "") {
            var promisePut = MessageService.Reply($scope.Reply);
            promisePut.then(
                function (result) {
                    if (result.data.Status === "success") {
                        $scope.Conversation.MessageList.push(result.data.Data);
                        $scope.Reply.Content = "";
                        toastr.success('Bạn đã gửi tin nhắn thành công!', 'Thành công!');
                    } else {
                        CommmonService.checkError(result.data.Type);
                        $scope.Error = result.data.Message;
                        toastr.error($scope.Error, 'Lỗi!');
                    }
                },
                function (error) {
                    $scope.Error = error.data.Message;
                    $scope.Error = result.data.Message;
                    toastr.error($scope.Error, 'Lỗi!');
                });
        } else {
            toastr.warning("Bạn chưa nhập nội dung tin nhắn", 'Thông báo!');
        }
    }

    // function to delete message.
    $scope.Delete = function () {
        var promise = MessageService.Delete($scope.Conversation.ConversationID);
        promise.then(
            function (result) {
                if (result.data.Status === "success") {
                    $location.path("/user/message").replace();
                } else {
                    CommmonService.checkError(result.data.Type);
                    $scope.Error = result.data.Message;
                    toastr.error($scope.Error, 'Lỗi!');
                }
            },
            function (error) {
                $scope.Error = error.data.Message;
                $scope.Error = result.data.Message;
                toastr.error($scope.Error, 'Lỗi!');
            });
    }
});