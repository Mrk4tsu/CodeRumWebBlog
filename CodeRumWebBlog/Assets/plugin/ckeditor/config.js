/**
 * @license Copyright (c) 2003-2023, CKSource Holding sp. z o.o. All rights reserved.
 * For licensing, see https://ckeditor.com/legal/ckeditor-oss-license
 */

CKEDITOR.editorConfig = function( config ) {
	// Define changes to default configuration here. For example:
    // config.language = 'fr';

    config.extraPlugins = "N1ED-editor";
    config.apiKey = "jNjsRlRj";
    config.skin = 'n1theme';
    config.extraPlugins = 'codesnippet';
    config.syntaxhighlight_lang = 'csharp';
    config.syntaxhighlight_hideControls = true;
    config.language = 'vi';
    config.codeSnippet_theme = 'school_book';
    config.filebrowserBrowseUrl = '/Assets/plugin/ckfinder/ckfinder.html';
    config.filebrowserImageBrowseUrl = '/Assets/plugin/ckfinder.html?Type=Images';
    config.filebrowserFlashBrowseUrl = '/Assets/plugin/ckfinder.html?Type=Flash';
    config.filebrowserUploadUrl = '/Assets/plugin/ckfinder/core/connector/aspx/connector.aspx?command=QuickUpload&type=Files';
    config.filebrowserImageUploadUrl = '/Assets/data';
    config.filebrowserFlashUploadUrl = '/Assets/plugin/ckfinder/core/connector/aspx/connector.aspx?command=QuickUpload&type=Flash';

    CKFinder.setupCKEditor(null, '/Assets/plugin/ckfinder/');
};
