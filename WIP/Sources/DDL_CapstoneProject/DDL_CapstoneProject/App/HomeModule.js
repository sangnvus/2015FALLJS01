"use strict";
var service = angular.module("DDLService", []);
var directive = angular.module("DDLDirective", []);
var app = angular.module("ClientApp", ["ngRoute", "ngAnimate", "ngSanitize", "DDLService",
    "DDLDirective", 'angular-loading-bar', 'textAngular', 'toastr']);

// Show Routing.
app.config(["$routeProvider", function ($routeProvider) {
    $routeProvider.when("/home",
        {
            redirectTo: "/"
        });
    $routeProvider.when("/",
        {
            templateUrl: "/ClientPartial/Home",
            controller: 'HomeController',
            resolve: {
                slides: ['SlideService', function (SlideService) {
                    return SlideService.getSlides();
                }]
            }
        });
    $routeProvider.when("/register",
        {
            templateUrl: "ClientPartial/Register"
        });
    $routeProvider.when("/register_success",
        {
            templateUrl: "ClientPartial/RegisterSuccess"
        });

    $routeProvider.when("/user/message",
        {
            templateUrl: "ClientPartial/Message",
            controller: "MessageController",
            resolve: {
                conversations: ['$rootScope', '$q', 'MessageService', 'CommmonService', function ($rootScope, $q, MessageService, CommmonService) {
                    var promise = MessageService.getListReceivedConversations();
                    return CommmonService.checkHttpResult($q, promise, $rootScope.BaseUrl);
                }]
            }
        });
    $routeProvider.when("/user/message/:id",
        {
            templateUrl: "ClientPartial/MessageDetail",
            controller: "MessageDetailController",
            resolve: {
                conversation: ['$rootScope','$route', '$q', 'MessageService', 'CommmonService', function ($rootScope,$route, $q, MessageService, CommmonService) {
                    var promise = MessageService.getConversation($route.current.params.id);
                    return CommmonService.checkHttpResult($q, promise, $rootScope.BaseUrl);
                }]
            }
        });
    $routeProvider.when("/create", {
        templateUrl: "/ClientPartial/CreateProject",
        controller: "CreateProjectController",
        resolve: {
            categories: ['CategoryService', function (CategoryService) {
                return CategoryService.getCategories();
            }]
        }
    });
    $routeProvider.when("/edit/:id",
        {
            templateUrl: "/ClientPartial/EditProject",
            controller: "EditProjectController"
        });
    $routeProvider.otherwise({
        redirectTo: "/"
    });

    //$locationProvider.html5Mode(false).hashPrefix("!");
}]).config(['$provide', function ($provide) {
    $provide.decorator('taOptions', ['taRegisterTool', '$delegate', function (taRegisterTool, taOptions) {

        taOptions.forceTextAngularSanitize = false;

        return taOptions;
    }]);
}]);

app.run(['$rootScope', '$window', '$anchorScroll', function ($rootScope, $window, $anchorScroll) {
    $rootScope.$on('$routeChangeError', function (e, curr, prev) {
        e.preventDefault();
    });

    // Base Url of web app.
    $rootScope.BaseUrl = angular.element($('#BaseUrl')).val();

    // Scroll top when route change.
    $rootScope.$on("$locationChangeStart", function () {
        $anchorScroll();
    });
}]);