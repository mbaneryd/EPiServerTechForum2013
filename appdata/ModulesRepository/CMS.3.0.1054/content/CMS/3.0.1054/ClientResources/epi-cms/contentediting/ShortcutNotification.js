//>>built
define("epi-cms/contentediting/ShortcutNotification",["dojo/_base/declare","dojo/_base/lang","epi/string","epi-cms/contentediting/_ContentEditingNotification","epi/i18n!epi/cms/nls/episerver.cms.contentediting.shortcutnotification"],function(_1,_2,_3,_4,_5){var _6=["normal","shortcut","external","inactive","fetchdata"];var _7=_1([_4],{contentData:null,postscript:function(){this.inherited(arguments);this.watch("contentData",_2.hitch(this,this._contentDataWatchHandler));},_onExecuteSuccess:function(_8){if(this._viewModelWatch){this._viewModelWatch.unwatch();}this._viewModelWatch=_8.watch("contentModel",_2.hitch(this,function(_9,_a,_b){if(this._contentModelWatch){this._contentModelWatch.unwatch();}this._contentModelWatch=_b.watch("iversionable_shortcut",_2.hitch(this,function(_c,_d,_e){this._updateNotification(_e&&_e.pageShortcutType);}));}));this.set("contentData",_8.contentData);},_contentDataWatchHandler:function(_f,_10,_11){if(_11&&_11.capabilities&&_11.capabilities.isPage){this._updateNotification(_11.properties.pageShortcutType);}},_updateNotification:function(_12){var _13,_14=null;if(_12>0){_13=_6[_12];_14=_3.toHTML(_5[_13]);}this._setNotification({shortcutType:_12,content:_14});}});_7.LinkTypes=_6;return _7;});