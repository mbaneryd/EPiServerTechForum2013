//>>built
define("epi-cms/contentediting/editors/_ContentAreaTree",["dojo/_base/declare","dojo/aspect","dojo/_base/event","dojo/_base/array","dojo/_base/lang","dojo/dom-class","dijit/registry","epi/dependency","epi/shell/conversion/ObjectConverterRegistry","epi/shell/dnd/tree/dndSource","epi/shell/TypeDescriptorManager","epi/shell/widget/Tree","epi-cms/contentediting/viewmodel/ContentBlockViewModel","epi-cms/contentediting/viewmodel/PersonalizedGroupViewModel","./_BlockTreeNode","./_GroupTreeNode"],function(_1,_2,_3,_4,_5,_6,_7,_8,_9,_a,_b,_c,_d,_e,_f,_10){return _1([_c],{showRoot:false,dndController:_a,betweenThreshold:8,postMixInProperties:function(){this.inherited(arguments);this.dndParams.push("accept");this.accept.push("personalizationarea");},postCreate:function(){this.inherited(arguments);_6.add(this.domNode,"epi-tree-mngr");this.own(_2.after(this.dndController,"onDndItemRemoved",_5.hitch(this,function(_11){if(_11[0]&&_11[0].data){this.model.deleteItem(_11[0].data);}}),true));this.own(_2.after(this,"_onItemChildrenChange",_5.hitch(this,function(_12,_13){_4.forEach(_13,function(_14){var _15=this._itemNodesMap[this.model.getIdentity(_14)];if(_14.expandOnAdd){this._expandNode(_15[0]);}if(_14.contentGroup&&_14.ensurePersonalization&&_14.hasAnyRoles&&!_14.hasAnyRoles()){_15[0].showPersonalizationSelector();}},this);}),true));},_setReadOnlyAttr:function(_16){this._set("readOnly",_16);if(this.dndController){this.dndController.isSource=!_16;}},getItemType:function(_17){if(!_17.typeIdentifier){if(_17.contentGroup!==undefined){return ["personalizationarea"];}else{return null;}}return this.accept;},checkItemAcceptance:function(_18,_19,_1a,_1b){if(this.tree.readOnly){return false;}var _1c=_7.getEnclosingWidget(_18);var _1d=_1c.item;var _1e=true;_19.forInSelectedItems(function(_1f){_1e=this.tree.model.checkItemAcceptance(_1d,_1f.data.item,_1a);},this);return _1e;},_onNodeMouseEnter:function(_20){_20.set("mouseHover",true);},_onNodeMouseLeave:function(_21){_21.set("mouseHover",false);},_createTreeNode:function(_22){var _23;if(_22.item instanceof _e){_23=new _10(_22);}else{if(_22.item instanceof _d){_23=new _f(_22);}else{_23=this.inherited(arguments);}}_23.set("contextMenu",this.contextMenu);_23.set("dndData",_22.item);return _23;},onClick:function(_24,_25,e){if(_25 instanceof _10){if(_25.isExpanded){this._collapseNode(_25);}else{this._expandNode(_25);}}}});});