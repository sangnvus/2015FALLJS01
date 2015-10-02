"use strict";

app.service('FacebookService', function ($http,$rootScope) {
    this.watchLoginStatusChange = function () {
        FB.Event.subscribe('auth.authResponseChange', function(response) {
            if (response.status == 'connected') {
                FB.api('/me?fields=email', function(res) {
                    $rootScope.$apply(function() {
                        if (!res || res.error) {
                            alert('Error occured');
                        } else {
                            alert(JSON.stringify(res));
                        }
                    });
                });
                FB.api('/me/permissions', function(res) {
                    $rootScope.$apply(function() {
                        if (!res || res.error) {
                            alert('Error occured');
                        } else {
                            //alert(res);
                        }
                    });
                });
            } else if (response.status == 'not_authorized') {
                // The person is logged into Facebook, but not your app.
                alert('Please log ' +
                    'into this app.');
            } else {
                // The person is not logged into Facebook, so we're not sure if
                // they are logged into this app or not.
                alert('Please log ' +
                    'into Facebook.');
            }
        });
    }

    this.Login = function() {
        FB.login(function (response) {
            if (response.status == 'connected') {
                FB.api('/me?fields=email', function (res) {
                    $rootScope.$apply(function () {
                        if (!res || res.error) {
                            alert('Error occured');
                        } else {
                            alert(JSON.stringify(res));
                        }
                    });
                });
                FB.api('/me/permissions', function (res) {
                    $rootScope.$apply(function () {
                        if (!res || res.error) {
                            alert('Error occured');
                        } else {
                            //alert(res);
                        }
                    });
                });
            } else if (response.status == 'not_authorized') {
                // The person is logged into Facebook, but not your app.
                alert('Please log ' +
                    'into this app.');
            } else {
                // The person is not logged into Facebook, so we're not sure if
                // they are logged into this app or not.
                alert('Please log ' +
                    'into Facebook.');
            }
        });
    }
});