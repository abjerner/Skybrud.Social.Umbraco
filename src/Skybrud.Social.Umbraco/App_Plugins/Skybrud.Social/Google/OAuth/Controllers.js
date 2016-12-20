angular.module("umbraco").controller("Skybrud.Social.Google.OAuth.Controller", ['$scope', 'editorState', function ($scope, editorState) {

    // Define an alias for the editor (eg. used for callbacks)
    var alias = ('skybrudsocial_' + Math.random()).replace('.', '');

    // Get a reference to the current editor state
    var state = editorState.current;

    $scope.callback = function (data) {
        $scope.model.value = data;
    };

    $scope.authorize = function () {

        var url = '/App_Plugins/Skybrud.Social/Dialogs/GoogleOAuth.aspx?callback=' + alias;
        url += "&contentTypeAlias=" + state.contentTypeAlias;
        url += "&propertyAlias=" + $scope.model.alias;

        window.open(url, 'Google OAuth', 'scrollbars=no,resizable=yes,menubar=no,width=800,height=600');

    };

    $scope.clear = function () {

        $scope.model.value = null;

    };

    // Register the callback function in the global scope
    window[alias] = $scope.callback;

}]);

angular.module("umbraco").controller("Skybrud.Social.Google.OAuth.PreValues.Controller", ['$scope', '$http', 'dialogService', function ($scope, $http, dialogService) {

    if (!$scope.model.value) {
        $scope.model.value = {
            appid: '',
            appsecret: '',
            redirecturi: '',
            scope: ['email', 'openid', 'profile']
        };
    }

    if (!$scope.model.value.scope) $scope.model.value.scope = [];

    $scope.addScope = function () {

        var d = dialogService.open({
            modalClass: 'SocialDialog',
            template: '/App_Plugins/Skybrud.Social/Google/OAuth/ScopesDialog.html',
            show: true,
            callback: function (scopes) {
                $scope.model.value.scope = scopes;
            },
            selection: $scope.model.value.scope
        });

        d.element[0].style.width = '1000px';
        d.element[0].style.marginLeft = '-500px';

    };

    $scope.removeScope = function (index) {
        $scope.model.value.scope.splice(index, 1);
    };

    $scope.suggestedRedirectUri = window.location.origin + '/App_Plugins/Skybrud.Social/Dialogs/GoogleOAuth.aspx';

}]);