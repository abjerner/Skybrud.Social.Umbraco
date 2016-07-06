angular.module("umbraco").controller("Skybrud.Social.Instagram.OAuth.Controller", ['$scope', 'editorState', function ($scope, editorState) {

    // Define an alias for the editor (eg. used for callbacks)
    var alias = ('skybrudsocial_' + Math.random()).replace('.', '');

    // Get a reference to the current editor state
    var state = editorState.current;

    $scope.callback = function (data) {
        $scope.$apply(function () {
            $scope.model.value = data;
        });
    };

    $scope.authorize = function () {

        var url = '/App_Plugins/Skybrud.Social/Dialogs/InstagramOAuth.aspx?callback=' + alias;
        url += "&contentTypeAlias=" + state.contentTypeAlias;
        url += "&propertyAlias=" + $scope.model.alias;

        window.open(url, 'Instagram OAuth', 'scrollbars=no,resizable=yes,menubar=no,width=800,height=600');

    };

    $scope.clear = function () {

        $scope.model.value = null;

    };

    // Register the callback function in the global scope
    window[alias] = $scope.callback;

}]);

angular.module("umbraco").controller("Skybrud.Social.Instagram.OAuth.PreValues.Controller", ['$scope', 'assetsService', function ($scope, assetsService) {

    if (!$scope.model.value) {
        $scope.model.value = {
            clientid: '',
            clientsecret: '',
            redirecturi: '',
            scope: ''
        };
    }

    $scope.scopes = [
        { alias: 'public_content', name: 'Public Content' },
        { alias: 'follower_list', name: 'Follower list' },
        { alias: 'comments', name: 'Comments' },
        { alias: 'relationships', name: 'Relationships' },
        { alias: 'likes', name: 'Likes' }
    ];

    $scope.init = function () {
        var temp = $scope.model.value.scope ? $scope.model.value.scope.split(',') : [];
        angular.forEach($scope.scopes, function (s) {
            s.selected = temp.indexOf(s.alias) >= 0;
        });
    };

    $scope.updateScope = function () {
        var temp = [];
        angular.forEach($scope.scopes, function (s) {
            if (s.selected) {
                temp.push(s.alias);
            }
        });
        $scope.model.value.scope = temp.join(',');
    };

    $scope.suggestedRedirectUri = window.location.origin + '/App_Plugins/Skybrud.Social/Dialogs/InstagramOAuth.aspx';

    $scope.init();

}]);