//>>built
define("epi-cms/widget/_Droppable",["dojo/_base/declare","dojo/_base/lang","dojo/when","epi/shell/dnd/Target"],function(_1,_2,_3,_4){return _1(null,{dropAreaNode:null,allowedDndTypes:null,postCreate:function(){this.inherited(arguments);var _5=this.own(new _4(this.dropAreaNode,{accept:this.allowedDndTypes,createItemOnDrop:false,readOnly:this.readOnly,skipForm:true}))[0];this.connect(_5,"onDropData","_onDropData");},onDropping:function(){},onDrop:function(){},_onDropData:function(_6,_7,_8,_9){var _a=_6?(_6.length?_6[0]:_6):null;if(_a){this.onDropping();_3(_a.data,_2.hitch(this,function(_b){var _c;if(this.dndSourcePropertyName){_c=_b[this.dndSourcePropertyName];}else{_c=_b;}this.set("value",_c);this.onDrop();}));}}});});