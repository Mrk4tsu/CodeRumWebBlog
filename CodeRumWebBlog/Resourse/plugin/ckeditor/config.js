/**
 * @license Copyright (c) 2003-2023, CKSource Holding sp. z o.o. All rights reserved.
 * For licensing, see https://ckeditor.com/legal/ckeditor-oss-license
 */

CKEDITOR.editorConfig = function( config ) {
	// Define changes to default configuration here. For example:
    // config.language = 'fr';

    /*============
    Add plugin
    ============*/
    config.extraPlugins = 'codesnippet,codeTag,tableresize,videodetector,youtube';

    /*============
    Remove plugin
    ============*/
    config.removePlugins = 'div,a11yhelp,about,magicline,pagebreak,pastetools,scayt,widget,iframe,templates,copyformatting,forms,pastefromword,pastefromlibreoffice,pastefromgdocs,find,selectall,print,magicline,newpage';


    /*============
    Set theme
    ============*/
    config.skin = 'n1theme';

    

    config.syntaxhighlight_lang = 'csharp';
    config.syntaxhighlight_hideControls = true;
    config.language = 'en';
    config.codeSnippet_theme = 'school_book';
    config.filebrowserBrowseUrl = '/Resourse/plugin/ckfinder/ckfinder.html';
    config.filebrowserImageBrowseUrl = '/Resourse/plugin/ckfinder.html?Type=Images';
    config.filebrowserFlashBrowseUrl = '/Resourse/plugin/ckfinder.html?Type=Flash';
    config.filebrowserUploadUrl = '/Resourse/plugin/ckfinder/core/connector/aspx/connector.aspx?command=QuickUpload&type=Files';
    config.filebrowserImageUploadUrl = '/Resourse/data';
    config.filebrowserFlashUploadUrl = '/Resourse/plugin/ckfinder/core/connector/aspx/connector.aspx?command=QuickUpload&type=Flash';

    CKFinder.setupCKEditor(null, '/Resourse/plugin/ckfinder/');
};
