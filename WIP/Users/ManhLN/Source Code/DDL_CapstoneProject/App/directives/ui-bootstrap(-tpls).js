angular.module('ui.bootstrap.tabs').config(['$provide', function ($provide) {
    $provide.decorator('tabDirective', ['$delegate', '$parse', function ($delegate, $parse) {
        var directive = $delegate[0];
        var compile = directive.compile;

        directive.compile = function () {
            var link = compile.apply(this, arguments);

            return function (scope, elm, attrs) {
                link.apply(this, arguments);

                // expose 'before-select' attribute as scope.beforeSelect
                scope.beforeSelect = angular.noop;
                attrs.$observe('beforeSelect', function (value) {
                    if (value) {
                        var parentGet = $parse(value);
                        scope.beforeSelect = function (locals) {
                            return parentGet(scope.$parent, locals);
                        };
                    } else {
                        scope.beforeSelect = angular.noop;
                    }
                });

                scope.select = function ($event) {
                    if (!scope.disabled) {
                        // abort tab switch if necessary
                        scope.beforeSelect({ $event: $event });
                        if ($event.isDefaultPrevented()) { return; }

                        scope.active = true;
                    }
                };
            };
        };

        return $delegate;

    }]);
}]);

angular.module("template/tabs/tab.html", []).run(["$templateCache", function ($templateCache) {
    $templateCache.put("template/tabs/tab.html",
      "<li ng-class=\"{active: active, disabled: disabled}\">\n" +
      "  <a href ng-click=\"select($event)\" tab-heading-transclude>{{heading}}</a>\n" +
      "</li>\n" +
      "");
}]);