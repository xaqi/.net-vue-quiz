function ZoomPic()
{this.initialize.apply(this,arguments)}
ZoomPic.prototype={initialize:function(id)
{var _this=this;this.wrap=typeof id==="string"?document.getElementById(id):id;this.oUl=this.wrap.getElementsByTagName("ul")[0];this.aLi=this.wrap.getElementsByTagName("li");this.prev=this.wrap.getElementsByTagName("pre")[0];this.next=this.wrap.getElementsByTagName("pre")[1];this.timer=null;this.aSort=[];this.iCenter=3;this._doPrev=function(){return _this.doPrev.apply(_this)};this._doNext=function(){return _this.doNext.apply(_this)};this.options=[{width:90,height:103,top:50,left:0,zIndex:1},{width:90,height:103,top:50,left:0,zIndex:2},{width:90,height:103,top:50,left:80,zIndex:3},{width:153,height:175,top:25,left:120,zIndex:4},{width:90,height:103,top:50,left:240,zIndex:3},{width:90,height:103,top:50,left:320,zIndex:2},{width:90,height:103,top:50,left:400,zIndex:1},];for(var i=0;i<this.aLi.length;i++)this.aSort[i]=this.aLi[i];this.aSort.unshift(this.aSort.pop());this.setUp();this.addEvent(this.prev,"click",this._doPrev);this.addEvent(this.next,"click",this._doNext);this.doImgClick();this.timer=setInterval(function()
{_this.doNext()},500);this.wrap.onmouseover=function()
{clearInterval(_this.timer)};this.wrap.onmouseout=function()
{_this.timer=setInterval(function()
{_this.doNext()},500);}},doPrev:function()
{this.aSort.unshift(this.aSort.pop());this.setUp()},doNext:function()
{this.aSort.push(this.aSort.shift());this.setUp()},doImgClick:function()
{var _this=this;for(var i=0;i<this.aSort.length;i++)
{this.aSort[i].onclick=function()
{if(this.index>_this.iCenter)
{for(var i=0;i<this.index-_this.iCenter;i++)_this.aSort.push(_this.aSort.shift());_this.setUp()}
else if(this.index<_this.iCenter)
{for(var i=0;i<_this.iCenter-this.index;i++)_this.aSort.unshift(_this.aSort.pop());_this.setUp()}}}},setUp:function()
{var _this=this;var i=0;for(i=0;i<this.aSort.length;i++)this.oUl.appendChild(this.aSort[i]);for(i=0;i<this.aSort.length;i++)
{this.aSort[i].index=i;if(i<7)
{this.css(this.aSort[i],"display","block");this.doMove(this.aSort[i],this.options[i],function()
{_this.doMove(_this.aSort[_this.iCenter].getElementsByTagName("img")[0],{opacity:100},function()
{_this.doMove(_this.aSort[_this.iCenter].getElementsByTagName("img")[0],{opacity:100},function()
{_this.aSort[_this.iCenter].onmouseover=function()
{_this.doMove(this.getElementsByTagName("div")[0],{bottom:0})};_this.aSort[_this.iCenter].onmouseout=function()
{_this.doMove(this.getElementsByTagName("div")[0],{bottom:-100});}})})});}
else
{this.css(this.aSort[i],"display","none");this.css(this.aSort[i],"width",0);this.css(this.aSort[i],"height",0);this.css(this.aSort[i],"top",50);this.css(this.aSort[i],"left",this.oUl.offsetWidth/2)}
if(i<this.iCenter||i>this.iCenter)
{this.css(this.aSort[i].getElementsByTagName("img")[0],"opacity",30)
this.aSort[i].onmouseover=function()
{_this.doMove(this.getElementsByTagName("img")[0],{opacity:100})};this.aSort[i].onmouseout=function()
{_this.doMove(this.getElementsByTagName("img")[0],{opacity:35})};this.aSort[i].onmouseout();}
else
{this.aSort[i].onmouseover=this.aSort[i].onmouseout=null}}},addEvent:function(oElement,sEventType,fnHandler)
{return oElement.addEventListener?oElement.addEventListener(sEventType,fnHandler,false):oElement.attachEvent("on"+sEventType,fnHandler)},css:function(oElement,attr,value)
{if(arguments.length==2)
{return oElement.currentStyle?oElement.currentStyle[attr]:getComputedStyle(oElement,null)[attr]}
else if(arguments.length==3)
{switch(attr)
{case"width":case"height":case"top":case"left":case"bottom":oElement.style[attr]=value+"px";break;default:oElement.style[attr]=value;break}}},doMove:function(oElement,oAttr,fnCallBack)
{var _this=this;clearInterval(oElement.timer);oElement.timer=setInterval(function()
{var bStop=true;for(var property in oAttr)
{var iCur=parseFloat(_this.css(oElement,property));property=="opacity"&&(iCur=parseInt(iCur.toFixed(2)*100));var iSpeed=(oAttr[property]-iCur)/5;iSpeed=iSpeed>0?Math.ceil(iSpeed):Math.floor(iSpeed);if(iCur!=oAttr[property])
{bStop=false;_this.css(oElement,property,iCur+iSpeed)}}
if(bStop)
{clearInterval(oElement.timer);fnCallBack&&fnCallBack.apply(_this,arguments)}},30)}};function chekmsg(o)
{if(o.contact.value=="")
{alert('请填写昵称')
return false}
if(o.qq.value=="")
{alert('请填写QQ')
return false;}
if(o.title.value=="")
{alert('请填写主题')
return false;}
if(o.content.value=="")
{alert('请填写内容')
return false}
if(o.code.value=="")
{alert('请填写验证码')
return false}}
function chekbook(o)
{if(o.contact.value=="")
{alert('请填写姓名')
return false;}
if(o.tel.value=="")
{alert('请填写联系方式')
return false;}}
function getvideo(c,i)
{$.post("server.php",{action:'getvideo',channel:c,id:i},function(data){arr=data.split("||")
str='<embed id="vshow" src="'+arr[0]+'" quality="high" width="548" height="334" align="middle" allowScriptAccess="always" allowFullScreen="true" mode="transparent" type="application/x-shockwave-flash"></embed>'
$("#v").html(str)})}
function loads()
{$('#slides').slides({preload:false,preloadImage:'/images/loading.gif',effect:'fade',slideSpeed:400,fadeSpeed:100,play:3000,pause:100,hoverPause:true});}
function nTabs(thisObj,Num)
{if(thisObj.className=="active")return;var tabObj=thisObj.parentNode.id;var tabList=document.getElementById(tabObj).getElementsByTagName("li");for(i=0;i<tabList.length;i++)
{if(i==Num)
{thisObj.className="active";document.getElementById(tabObj+"_Content"+i).style.display="block";}
else
{tabList[i].className="normal";document.getElementById(tabObj+"_Content"+i).style.display="none";}}}