//>>built
require({cache:{"url:epi-cms/widget/templates/ContentReferences.html":"<div class=\"epi-contentReferences\">\r\n    <div data-dojo-attach-point=\"contentReferencesNotificationBar\" class=\"epi-notificationBar epi-notificationBarWithBorders epi-notificationBarItem\">\r\n        <div data-dojo-attach-point=\"contentReferencesNotificationNode\" class=\"epi-notificationBarText\" ></div>\r\n    </div>\r\n    <div class=\"epi-captionPanel\" data-dojo-attach-point=\"captionPanelNode\">\r\n        <strong data-dojo-attach-point=\"totalLinksNode\"></strong>\r\n        <div class=\"epi-floatRight\">\r\n            <button class=\"epi-chromelessButton\"\r\n                    data-dojo-type=\"dijit/form/Button\"\r\n                    data-dojo-attach-event=\"onClick:fetchData\"\r\n                    data-dojo-props=\"iconClass:'epi-iconReload'\">${res.buttons.refresh}</button>\r\n        </div>\r\n    </div>\r\n    <div data-dojo-attach-point=\"gridNode\"></div>\r\n</div>\r\n"}});define("epi-cms/widget/ContentReferences",["dojo/_base/declare","dojo/_base/lang","dojo/dom-class","dojo/dom-construct","dojo/dom-style","dojo/keys","dojo/when","epi-cms/dgrid/formatters","epi/shell/TypeDescriptorManager","./_GridWidgetBase","dijit/_TemplatedMixin","dijit/_WidgetsInTemplateMixin","dojo/Evented","dojo/text!./templates/ContentReferences.html","epi/i18n!epi/cms/nls/episerver.cms.widget.contentreferences","dijit/form/Button"],function(_1,_2,_3,_4,_5,_6,_7,_8,_9,_a,_b,_c,_d,_e,_f){return _1([_a,_b,_c,_d],{res:_f,templateString:_e,storeKeyName:"epi.cms.contentreferences",contextChangeEvent:"dblclick",trackActiveItem:false,ignoreVersionWhenComparingLinks:false,_setTotalLinksAttr:{node:"totalLinksNode",type:"innerText"},_setNotificationAttr:{node:"contentReferencesNotificationNode",type:"innerText"},_setNotificationBarStyleAttr:function(_10){switch(_10){case 1:_3.add(this.contentReferencesNotificationBar,"epi-statusIndicator epi-statusIndicatorOk");break;case 2:_3.add(this.contentReferencesNotificationBar,"epi-statusIndicator epi-statusIndicatorWarning");break;}},_setNumberOfReferencesAttr:function(_11){var _12=this.model.contentData,_13=_11>0;var _14=_2.hitch(this,function(_15,_16){var _17=_9.getResourceValue(_12.typeIdentifier,_15);this.set("notification",_17);this.set("notificationBarStyle",_16);});if(_13){if(_12.hasChildren){_14("referenceswithsubcontentwarning",2);}else{_14("referenceswarning",2);}}else{if(_12.hasChildren){_14("noreferenceswithsubcontentwarning",null);}else{if(_12.publicUrl){_14("publicreferencesnote",null);}else{_14("noreferencesnote",1);}}}if(_13){var _18=_11===1?_f.totallink:_f.totallinks;var _19=_2.replace(_18,[_11]);this.set("totalLinks",_19);}this._set("numberOfReferences",_11);},_setShowToolbarAttr:function(_1a){_5.set(this.captionPanelNode,"display",_1a?"":"none");},buildRendering:function(){var _1b=this._getLinkTemplate();var _1c=_2.mixin({columns:{name:{renderCell:_2.hitch(this,"_renderContentItem")},treePath:{className:"epi-width50",get:function(_1d){return _1d.treePath.join("<span class=\"epi-breadCrumbsSeparator\">&gt;</span>");},ellipsisNoTooltip:true},view:{className:"epi-width10",label:" ",formatter:function(){return _1b;},sortable:false}},store:this.store,query:{id:this.model.contentData.contentLink},showHeader:false},this.defaultGridMixin);this.inherited(arguments);this.grid=new this._gridClass(_1c,this.gridNode);_3.toggle(this.contentReferencesNotificationBar,"dijitHidden",this.model.mode=="show");},startup:function(){this.inherited(arguments);this.grid.on(".dgrid-column-view a:click",_2.hitch(this,"_onChangeContext"));this.grid.on("dgrid-refresh-complete",_2.hitch(this,"_afterDataFetched"));},fetchData:function(){this.grid.refresh();},_afterDataFetched:function(e){_7(e.results.total,_2.hitch(this,function(_1e){this.set("numberOfReferences",_1e);this.set("showToolbar",_1e>0);_5.set(this.grid.domNode,"visibility",(_1e>0)?"visible":"hidden");}));},_onChangeContext:function(e){var row=this.grid.row(e),_1f={uri:row.data.uri};this._requestNewContext(_1f,{sender:this});this.emit("viewReference");},_getLinkTemplate:function(){var _20=_4.create("a",{"class":"epi-visibleLink",innerHTML:_f.view.label,title:_f.view.tooltip});return _20.outerHTML;}});});