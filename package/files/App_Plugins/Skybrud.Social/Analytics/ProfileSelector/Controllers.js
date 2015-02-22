angular.module("umbraco").controller("Skybrud.Social.Analytics.ProfileSelector.PreValues.Controller", ['$scope', '$http', 'assetsService', function ($scope, $http, assetsService) {

    // Define an alias for the editor (eg. used for callbacks)
    var alias = ('skybrudsocial_' + Math.random()).replace('.', '');

    $scope.modes = [
        {alias: 'config', name: 'Configuration'},
        {alias: 'editor', name: 'Editor'}
    ];

    $scope.mode = $scope.modes[0].alias;

    if (!$scope.model.value) {
        $scope.model.value = {
            appid: '',
            appsecret: '',
            redirecturi: '',
            mode: 'config',
            refreshToken: '',
            user: null
        };
    }

    $scope.suggestedRedirectUri = window.location.origin + '/App_Plugins/Skybrud.Social/Dialogs/GoogleOAuth.aspx';

    assetsService.loadCss("/App_Plugins/Skybrud.Social/Social.css");

    $scope.callback = function (data) {
        $scope.model.refreshToken = data.refreshToken;
        $scope.model.user = data.user;
    };

    $scope.authorize = function () {

        alert('Yeah');
        return;

        var url = '/App_Plugins/Skybrud.Social/Dialogs/GoogleOAuth.aspx?callback=' + alias;
        url += "&contentTypeAlias=" + state.contentTypeAlias;
        url += "&propertyAlias=" + $scope.model.alias;

        window.open(url, 'Google OAuth', 'scrollbars=no,resizable=yes,menubar=no,width=800,height=600');

    };

    $scope.clear = function () {

        $scope.model.refreshtoken = '';
        $scope.model.user = null;

    };

    // Register the callback function in the global scope
    window[alias] = $scope.callback;

}]);