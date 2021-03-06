<?xml version='1.0'?>
<Project Type="Project" LVVersion="8208000">
   <Item Name="My Computer" Type="My Computer">
      <Property Name="CCSymbols" Type="Str">OS,Win;CPU,x86;</Property>
      <Property Name="server.app.propertiesEnabled" Type="Bool">true</Property>
      <Property Name="server.control.propertiesEnabled" Type="Bool">true</Property>
      <Property Name="server.tcp.enabled" Type="Bool">false</Property>
      <Property Name="server.tcp.port" Type="Int">0</Property>
      <Property Name="server.tcp.serviceName" Type="Str">My Computer/VI Server</Property>
      <Property Name="server.tcp.serviceName.default" Type="Str">My Computer/VI Server</Property>
      <Property Name="server.vi.callsEnabled" Type="Bool">true</Property>
      <Property Name="server.vi.propertiesEnabled" Type="Bool">true</Property>
      <Property Name="specify.custom.address" Type="Bool">false</Property>
      <Item Name="internet" Type="Folder">
         <Item Name="http" Type="Folder">
            <Item Name="conf" Type="Folder">
               <Item Name="access.cfg" Type="Document" URL="internet/http/conf/access.cfg"/>
               <Item Name="grp.txt" Type="Document" URL="internet/http/conf/grp.txt"/>
               <Item Name="lvhttp.cfg" Type="Document" URL="internet/http/conf/lvhttp.cfg"/>
               <Item Name="mime.typ" Type="Document" URL="internet/http/conf/mime.typ"/>
               <Item Name="pwd.txt" Type="Document" URL="internet/http/conf/pwd.txt"/>
               <Item Name="srm.cfg" Type="Document" URL="internet/http/conf/srm.cfg"/>
            </Item>
            <Item Name="htdocs" Type="Folder">
               <Item Name="gweb.gif" Type="Document" URL="internet/http/htdocs/gweb.gif"/>
               <Item Name="index.htm" Type="Document" URL="internet/http/htdocs/index.htm"/>
               <Item Name="lvback3.gif" Type="Document" URL="internet/http/htdocs/lvback3.gif"/>
            </Item>
            <Item Name="icons" Type="Folder">
               <Item Name="back.gif" Type="Document" URL="internet/http/icons/back.gif"/>
               <Item Name="bin.gif" Type="Document" URL="internet/http/icons/bin.gif"/>
               <Item Name="binhex.gif" Type="Document" URL="internet/http/icons/binhex.gif"/>
               <Item Name="blank.gif" Type="Document" URL="internet/http/icons/blank.gif"/>
               <Item Name="dir.gif" Type="Document" URL="internet/http/icons/dir.gif"/>
               <Item Name="file.gif" Type="Document" URL="internet/http/icons/file.gif"/>
               <Item Name="llb.gif" Type="Document" URL="internet/http/icons/llb.gif"/>
               <Item Name="logo.gif" Type="Document" URL="internet/http/icons/logo.gif"/>
               <Item Name="movie.gif" Type="Document" URL="internet/http/icons/movie.gif"/>
               <Item Name="pict.gif" Type="Document" URL="internet/http/icons/pict.gif"/>
               <Item Name="sound.gif" Type="Document" URL="internet/http/icons/sound.gif"/>
               <Item Name="text.gif" Type="Document" URL="internet/http/icons/text.gif"/>
               <Item Name="vi.gif" Type="Document" URL="internet/http/icons/vi.gif"/>
            </Item>
            <Item Name="logs" Type="Folder">
               <Item Name="readme.txt" Type="Document" URL="internet/http/logs/readme.txt"/>
            </Item>
         </Item>
         <Item Name="internet.ini" Type="Document" URL="internet/internet.ini"/>
      </Item>
      <Item Name="www" Type="Folder">
         <Item Name="cgi-bin" Type="Folder">
            <Item Name="ILAB_FrameContentCGI.vi" Type="VI" URL="www/cgi-bin/ILAB_FrameContentCGI.vi"/>
         </Item>
         <Item Name="index.htm" Type="Document" URL="www/index.htm"/>
      </Item>
      <Item Name="user.lib" Type="Folder">
         <Item Name="iLabs" Type="Folder">
            <Item Name="ILAB_CaseHandler.vi" Type="VI" URL="user.lib/iLabs.llb/ILAB_CaseHandler.vi"/>
            <Item Name="ILAB_CreateFromTemplate.vi" Type="VI" URL="user.lib/iLabs.llb/ILAB_CreateFromTemplate.vi"/>
            <Item Name="ILAB_GetVI.vi" Type="VI" URL="user.lib/iLabs.llb/ILAB_GetVI.vi"/>
            <Item Name="ILAB_IsLoaded.vi" Type="VI" URL="user.lib/iLabs.llb/ILAB_IsLoaded.vi"/>
            <Item Name="ILAB_RemoteAppRef.vi" Type="VI" URL="user.lib/iLabs.llb/ILAB_RemoteAppRef.vi"/>
            <Item Name="ILAB_SetBounds.vi" Type="VI" URL="user.lib/iLabs.llb/ILAB_SetBounds.vi"/>
            <Item Name="ILAB_ShowStatus.vi" Type="VI" URL="user.lib/iLabs.llb/ILAB_ShowStatus.vi"/>
            <Item Name="ILAB_ViStatus.vi" Type="VI" URL="user.lib/iLabs.llb/ILAB_ViStatus.vi"/>
            <Item Name="ILABR_CaseHandler.vi" Type="VI" URL="user.lib/iLabs.llb/ILABR_CaseHandler.vi"/>
            <Item Name="ILABR_CreateFromTemplate.vi" Type="VI" URL="user.lib/iLabs.llb/ILABR_CreateFromTemplate.vi"/>
            <Item Name="ILABR_GetVI.vi" Type="VI" URL="user.lib/iLabs.llb/ILABR_GetVI.vi"/>
            <Item Name="ILABR_IsLoaded.vi" Type="VI" URL="user.lib/iLabs.llb/ILABR_IsLoaded.vi"/>
            <Item Name="ILABR_SetBounds.vi" Type="VI" URL="user.lib/iLabs.llb/ILABR_SetBounds.vi"/>
            <Item Name="ILABR_ViStatus.vi" Type="VI" URL="user.lib/iLabs.llb/ILABR_ViStatus.vi"/>
            <Item Name="ILABs_CaseHandler.vi" Type="VI" URL="user.lib/iLabs.llb/ILABs_CaseHandler.vi"/>
            <Item Name="ILABs_CreateFromTemplate.vi" Type="VI" URL="user.lib/iLabs.llb/ILABs_CreateFromTemplate.vi"/>
            <Item Name="ILABs_ErrorToString.vi" Type="VI" URL="user.lib/iLabs.llb/ILABs_ErrorToString.vi"/>
            <Item Name="ILABs_GetVI.vi" Type="VI" URL="user.lib/iLabs.llb/ILABs_GetVI.vi"/>
            <Item Name="ILABs_IsLoaded.vi" Type="VI" URL="user.lib/iLabs.llb/ILABs_IsLoaded.vi"/>
            <Item Name="ILABs_RemotePanelHandler.vi" Type="VI" URL="user.lib/iLabs.llb/ILABs_RemotePanelHandler.vi"/>
            <Item Name="ILABs_SetBounds.vi" Type="VI" URL="user.lib/iLabs.llb/ILABs_SetBounds.vi"/>
            <Item Name="ILABs_ViStatus.vi" Type="VI" URL="user.lib/iLabs.llb/ILABs_ViStatus.vi"/>
            <Item Name="viewDataSocket.vi" Type="VI" URL="user.lib/iLabs.llb/viewDataSocket.vi"/>
         </Item>
      </Item>
      <Item Name="Dependencies" Type="Dependencies"/>
      <Item Name="Build Specifications" Type="Build">
         <Item Name="iLab LabVIEW 8.2 Installation" Type="Source Distribution">
            <Property Name="Absolute[0]" Type="Bool">false</Property>
            <Property Name="Absolute[1]" Type="Bool">false</Property>
            <Property Name="BuildName" Type="Str">iLab LabVIEW 8.2 Installation</Property>
            <Property Name="DependencyApplyDestination" Type="Bool">true</Property>
            <Property Name="DependencyApplyInclusion" Type="Bool">true</Property>
            <Property Name="DependencyApplyPassword" Type="Bool">true</Property>
            <Property Name="DependencyApplyProperties" Type="Bool">true</Property>
            <Property Name="DependencyFolderDestination" Type="Int">0</Property>
            <Property Name="DependencyFolderInclusion" Type="Str">As Needed</Property>
            <Property Name="DependencyFolderMask" Type="Str">Default</Property>
            <Property Name="DependencyFolderPasswordSetting" Type="Str">No Password Change</Property>
            <Property Name="DependencyFolderPasswordToApply" Type="Str"></Property>
            <Property Name="DependencyFolderPropertiesItemCount" Type="Int">0</Property>
            <Property Name="DependencyFolderTopLevelVI" Type="Str">Never</Property>
            <Property Name="DestinationID[0]" Type="Str">{361E60BF-4147-4C42-9DC6-1B1EB39F0842}</Property>
            <Property Name="DestinationID[1]" Type="Str">{B1F11561-AFC0-480C-8D77-EC23182DBD3C}</Property>
            <Property Name="DestinationItemCount" Type="Int">2</Property>
            <Property Name="DestinationName[0]" Type="Str">Destination Directory</Property>
            <Property Name="DestinationName[1]" Type="Str">Support Directory</Property>
            <Property Name="DestinationOption" Type="Str">Preserve Hierarchy</Property>
            <Property Name="Disconnect" Type="Bool">false</Property>
            <Property Name="ExcludeInstrLib" Type="Bool">true</Property>
            <Property Name="ExcludeUserLib" Type="Bool">false</Property>
            <Property Name="ExcludeVILIB" Type="Bool">true</Property>
            <Property Name="Path[0]" Type="Path">../../../builds/LViLabsApp_82/iLab LabVIEW 8.2 Installation</Property>
            <Property Name="Path[1]" Type="Path">../../../builds/LViLabsApp_82/iLab LabVIEW 8.2 Installation/data</Property>
            <Property Name="SourceInfoItemCount" Type="Int">59</Property>
            <Property Name="SourceItem[0].FolderTopLevelVI" Type="Str">Never</Property>
            <Property Name="SourceItem[0].IsFolder" Type="Bool">true</Property>
            <Property Name="SourceItem[0].ItemID" Type="Ref">/My Computer/internet</Property>
            <Property Name="SourceItem[0].TopLevelVI" Type="Str">Never</Property>
            <Property Name="SourceItem[1].FolderTopLevelVI" Type="Str">Never</Property>
            <Property Name="SourceItem[1].IsFolder" Type="Bool">true</Property>
            <Property Name="SourceItem[1].ItemID" Type="Ref">/My Computer/internet/http</Property>
            <Property Name="SourceItem[1].TopLevelVI" Type="Str">Never</Property>
            <Property Name="SourceItem[10].FolderTopLevelVI" Type="Str">Never</Property>
            <Property Name="SourceItem[10].ItemID" Type="Ref">/My Computer/internet/http/htdocs/gweb.gif</Property>
            <Property Name="SourceItem[10].TopLevelVI" Type="Str">Never</Property>
            <Property Name="SourceItem[11].FolderTopLevelVI" Type="Str">Never</Property>
            <Property Name="SourceItem[11].ItemID" Type="Ref">/My Computer/internet/http/htdocs/index.htm</Property>
            <Property Name="SourceItem[11].TopLevelVI" Type="Str">Never</Property>
            <Property Name="SourceItem[12].FolderTopLevelVI" Type="Str">Never</Property>
            <Property Name="SourceItem[12].ItemID" Type="Ref">/My Computer/internet/http/htdocs/lvback3.gif</Property>
            <Property Name="SourceItem[12].TopLevelVI" Type="Str">Never</Property>
            <Property Name="SourceItem[13].FolderTopLevelVI" Type="Str">Never</Property>
            <Property Name="SourceItem[13].IsFolder" Type="Bool">true</Property>
            <Property Name="SourceItem[13].ItemID" Type="Ref">/My Computer/internet/http/icons</Property>
            <Property Name="SourceItem[13].TopLevelVI" Type="Str">Never</Property>
            <Property Name="SourceItem[14].FolderTopLevelVI" Type="Str">Never</Property>
            <Property Name="SourceItem[14].ItemID" Type="Ref">/My Computer/internet/http/icons/back.gif</Property>
            <Property Name="SourceItem[14].TopLevelVI" Type="Str">Never</Property>
            <Property Name="SourceItem[15].FolderTopLevelVI" Type="Str">Never</Property>
            <Property Name="SourceItem[15].ItemID" Type="Ref">/My Computer/internet/http/icons/bin.gif</Property>
            <Property Name="SourceItem[15].TopLevelVI" Type="Str">Never</Property>
            <Property Name="SourceItem[16].FolderTopLevelVI" Type="Str">Never</Property>
            <Property Name="SourceItem[16].ItemID" Type="Ref">/My Computer/internet/http/icons/binhex.gif</Property>
            <Property Name="SourceItem[16].TopLevelVI" Type="Str">Never</Property>
            <Property Name="SourceItem[17].FolderTopLevelVI" Type="Str">Never</Property>
            <Property Name="SourceItem[17].ItemID" Type="Ref">/My Computer/internet/http/icons/blank.gif</Property>
            <Property Name="SourceItem[17].TopLevelVI" Type="Str">Never</Property>
            <Property Name="SourceItem[18].FolderTopLevelVI" Type="Str">Never</Property>
            <Property Name="SourceItem[18].ItemID" Type="Ref">/My Computer/internet/http/icons/dir.gif</Property>
            <Property Name="SourceItem[18].TopLevelVI" Type="Str">Never</Property>
            <Property Name="SourceItem[19].FolderTopLevelVI" Type="Str">Never</Property>
            <Property Name="SourceItem[19].ItemID" Type="Ref">/My Computer/internet/http/icons/file.gif</Property>
            <Property Name="SourceItem[19].TopLevelVI" Type="Str">Never</Property>
            <Property Name="SourceItem[2].FolderTopLevelVI" Type="Str">Never</Property>
            <Property Name="SourceItem[2].IsFolder" Type="Bool">true</Property>
            <Property Name="SourceItem[2].ItemID" Type="Ref">/My Computer/internet/http/conf</Property>
            <Property Name="SourceItem[2].TopLevelVI" Type="Str">Never</Property>
            <Property Name="SourceItem[20].FolderTopLevelVI" Type="Str">Never</Property>
            <Property Name="SourceItem[20].ItemID" Type="Ref">/My Computer/internet/http/icons/llb.gif</Property>
            <Property Name="SourceItem[20].TopLevelVI" Type="Str">Never</Property>
            <Property Name="SourceItem[21].FolderTopLevelVI" Type="Str">Never</Property>
            <Property Name="SourceItem[21].ItemID" Type="Ref">/My Computer/internet/http/icons/logo.gif</Property>
            <Property Name="SourceItem[21].TopLevelVI" Type="Str">Never</Property>
            <Property Name="SourceItem[22].FolderTopLevelVI" Type="Str">Never</Property>
            <Property Name="SourceItem[22].ItemID" Type="Ref">/My Computer/internet/http/icons/movie.gif</Property>
            <Property Name="SourceItem[22].TopLevelVI" Type="Str">Never</Property>
            <Property Name="SourceItem[23].FolderTopLevelVI" Type="Str">Never</Property>
            <Property Name="SourceItem[23].ItemID" Type="Ref">/My Computer/internet/http/icons/pict.gif</Property>
            <Property Name="SourceItem[23].TopLevelVI" Type="Str">Never</Property>
            <Property Name="SourceItem[24].FolderTopLevelVI" Type="Str">Never</Property>
            <Property Name="SourceItem[24].ItemID" Type="Ref">/My Computer/internet/http/icons/sound.gif</Property>
            <Property Name="SourceItem[24].TopLevelVI" Type="Str">Never</Property>
            <Property Name="SourceItem[25].FolderTopLevelVI" Type="Str">Never</Property>
            <Property Name="SourceItem[25].ItemID" Type="Ref">/My Computer/internet/http/icons/text.gif</Property>
            <Property Name="SourceItem[25].TopLevelVI" Type="Str">Never</Property>
            <Property Name="SourceItem[26].FolderTopLevelVI" Type="Str">Never</Property>
            <Property Name="SourceItem[26].ItemID" Type="Ref">/My Computer/internet/http/icons/vi.gif</Property>
            <Property Name="SourceItem[26].TopLevelVI" Type="Str">Never</Property>
            <Property Name="SourceItem[27].FolderTopLevelVI" Type="Str">Never</Property>
            <Property Name="SourceItem[27].IsFolder" Type="Bool">true</Property>
            <Property Name="SourceItem[27].ItemID" Type="Ref">/My Computer/internet/http/logs</Property>
            <Property Name="SourceItem[27].TopLevelVI" Type="Str">Never</Property>
            <Property Name="SourceItem[28].FolderTopLevelVI" Type="Str">Never</Property>
            <Property Name="SourceItem[28].ItemID" Type="Ref">/My Computer/internet/http/logs/readme.txt</Property>
            <Property Name="SourceItem[28].TopLevelVI" Type="Str">Never</Property>
            <Property Name="SourceItem[29].FolderTopLevelVI" Type="Str">Never</Property>
            <Property Name="SourceItem[29].ItemID" Type="Ref">/My Computer/internet/internet.ini</Property>
            <Property Name="SourceItem[29].TopLevelVI" Type="Str">Never</Property>
            <Property Name="SourceItem[3].FolderTopLevelVI" Type="Str">Never</Property>
            <Property Name="SourceItem[3].ItemID" Type="Ref">/My Computer/internet/http/conf/access.cfg</Property>
            <Property Name="SourceItem[3].TopLevelVI" Type="Str">Never</Property>
            <Property Name="SourceItem[30].FolderTopLevelVI" Type="Str">Never</Property>
            <Property Name="SourceItem[30].IsFolder" Type="Bool">true</Property>
            <Property Name="SourceItem[30].ItemID" Type="Ref">/My Computer/www</Property>
            <Property Name="SourceItem[30].TopLevelVI" Type="Str">Never</Property>
            <Property Name="SourceItem[31].FolderTopLevelVI" Type="Str">Never</Property>
            <Property Name="SourceItem[31].IsFolder" Type="Bool">true</Property>
            <Property Name="SourceItem[31].ItemID" Type="Ref">/My Computer/www/cgi-bin</Property>
            <Property Name="SourceItem[31].TopLevelVI" Type="Str">Never</Property>
            <Property Name="SourceItem[32].FolderTopLevelVI" Type="Str">Never</Property>
            <Property Name="SourceItem[32].ItemID" Type="Ref">/My Computer/www/cgi-bin/ILAB_FrameContentCGI.vi</Property>
            <Property Name="SourceItem[32].TopLevelVI" Type="Str">Never</Property>
            <Property Name="SourceItem[33].FolderTopLevelVI" Type="Str">Never</Property>
            <Property Name="SourceItem[33].ItemID" Type="Ref">/My Computer/www/index.htm</Property>
            <Property Name="SourceItem[33].TopLevelVI" Type="Str">Never</Property>
            <Property Name="SourceItem[34].FolderTopLevelVI" Type="Str">Never</Property>
            <Property Name="SourceItem[34].IsFolder" Type="Bool">true</Property>
            <Property Name="SourceItem[34].ItemID" Type="Ref">/My Computer/user.lib</Property>
            <Property Name="SourceItem[34].TopLevelVI" Type="Str">Never</Property>
            <Property Name="SourceItem[35].FolderTopLevelVI" Type="Str">Never</Property>
            <Property Name="SourceItem[35].IsFolder" Type="Bool">true</Property>
            <Property Name="SourceItem[35].ItemID" Type="Ref">/My Computer/user.lib/iLabs</Property>
            <Property Name="SourceItem[35].TopLevelVI" Type="Str">Never</Property>
            <Property Name="SourceItem[36].FolderTopLevelVI" Type="Str">Never</Property>
            <Property Name="SourceItem[36].ItemID" Type="Ref">/My Computer/user.lib/iLabs/ILAB_CaseHandler.vi</Property>
            <Property Name="SourceItem[36].TopLevelVI" Type="Str">Never</Property>
            <Property Name="SourceItem[37].FolderTopLevelVI" Type="Str">Never</Property>
            <Property Name="SourceItem[37].ItemID" Type="Ref">/My Computer/user.lib/iLabs/ILAB_CreateFromTemplate.vi</Property>
            <Property Name="SourceItem[37].TopLevelVI" Type="Str">Never</Property>
            <Property Name="SourceItem[38].FolderTopLevelVI" Type="Str">Never</Property>
            <Property Name="SourceItem[38].ItemID" Type="Ref">/My Computer/user.lib/iLabs/ILAB_GetVI.vi</Property>
            <Property Name="SourceItem[38].TopLevelVI" Type="Str">Never</Property>
            <Property Name="SourceItem[39].FolderTopLevelVI" Type="Str">Never</Property>
            <Property Name="SourceItem[39].ItemID" Type="Ref">/My Computer/user.lib/iLabs/ILAB_IsLoaded.vi</Property>
            <Property Name="SourceItem[39].TopLevelVI" Type="Str">Never</Property>
            <Property Name="SourceItem[4].FolderTopLevelVI" Type="Str">Never</Property>
            <Property Name="SourceItem[4].ItemID" Type="Ref">/My Computer/internet/http/conf/grp.txt</Property>
            <Property Name="SourceItem[4].TopLevelVI" Type="Str">Never</Property>
            <Property Name="SourceItem[40].FolderTopLevelVI" Type="Str">Never</Property>
            <Property Name="SourceItem[40].ItemID" Type="Ref">/My Computer/user.lib/iLabs/ILAB_RemoteAppRef.vi</Property>
            <Property Name="SourceItem[40].TopLevelVI" Type="Str">Never</Property>
            <Property Name="SourceItem[41].FolderTopLevelVI" Type="Str">Never</Property>
            <Property Name="SourceItem[41].ItemID" Type="Ref">/My Computer/user.lib/iLabs/ILAB_SetBounds.vi</Property>
            <Property Name="SourceItem[41].TopLevelVI" Type="Str">Never</Property>
            <Property Name="SourceItem[42].FolderTopLevelVI" Type="Str">Never</Property>
            <Property Name="SourceItem[42].ItemID" Type="Ref">/My Computer/user.lib/iLabs/ILAB_ShowStatus.vi</Property>
            <Property Name="SourceItem[42].TopLevelVI" Type="Str">Never</Property>
            <Property Name="SourceItem[43].FolderTopLevelVI" Type="Str">Never</Property>
            <Property Name="SourceItem[43].ItemID" Type="Ref">/My Computer/user.lib/iLabs/ILAB_ViStatus.vi</Property>
            <Property Name="SourceItem[43].TopLevelVI" Type="Str">Never</Property>
            <Property Name="SourceItem[44].FolderTopLevelVI" Type="Str">Never</Property>
            <Property Name="SourceItem[44].ItemID" Type="Ref">/My Computer/user.lib/iLabs/ILABR_CaseHandler.vi</Property>
            <Property Name="SourceItem[44].TopLevelVI" Type="Str">Never</Property>
            <Property Name="SourceItem[45].FolderTopLevelVI" Type="Str">Never</Property>
            <Property Name="SourceItem[45].ItemID" Type="Ref">/My Computer/user.lib/iLabs/ILABR_CreateFromTemplate.vi</Property>
            <Property Name="SourceItem[45].TopLevelVI" Type="Str">Never</Property>
            <Property Name="SourceItem[46].FolderTopLevelVI" Type="Str">Never</Property>
            <Property Name="SourceItem[46].ItemID" Type="Ref">/My Computer/user.lib/iLabs/ILABR_GetVI.vi</Property>
            <Property Name="SourceItem[46].TopLevelVI" Type="Str">Never</Property>
            <Property Name="SourceItem[47].FolderTopLevelVI" Type="Str">Never</Property>
            <Property Name="SourceItem[47].ItemID" Type="Ref">/My Computer/user.lib/iLabs/ILABR_IsLoaded.vi</Property>
            <Property Name="SourceItem[47].TopLevelVI" Type="Str">Never</Property>
            <Property Name="SourceItem[48].FolderTopLevelVI" Type="Str">Never</Property>
            <Property Name="SourceItem[48].ItemID" Type="Ref">/My Computer/user.lib/iLabs/ILABR_SetBounds.vi</Property>
            <Property Name="SourceItem[48].TopLevelVI" Type="Str">Never</Property>
            <Property Name="SourceItem[49].FolderTopLevelVI" Type="Str">Never</Property>
            <Property Name="SourceItem[49].ItemID" Type="Ref">/My Computer/user.lib/iLabs/ILABR_ViStatus.vi</Property>
            <Property Name="SourceItem[49].TopLevelVI" Type="Str">Never</Property>
            <Property Name="SourceItem[5].FolderTopLevelVI" Type="Str">Never</Property>
            <Property Name="SourceItem[5].ItemID" Type="Ref">/My Computer/internet/http/conf/lvhttp.cfg</Property>
            <Property Name="SourceItem[5].TopLevelVI" Type="Str">Never</Property>
            <Property Name="SourceItem[50].FolderTopLevelVI" Type="Str">Never</Property>
            <Property Name="SourceItem[50].ItemID" Type="Ref">/My Computer/user.lib/iLabs/ILABs_CaseHandler.vi</Property>
            <Property Name="SourceItem[50].TopLevelVI" Type="Str">Never</Property>
            <Property Name="SourceItem[51].FolderTopLevelVI" Type="Str">Never</Property>
            <Property Name="SourceItem[51].ItemID" Type="Ref">/My Computer/user.lib/iLabs/ILABs_CreateFromTemplate.vi</Property>
            <Property Name="SourceItem[51].TopLevelVI" Type="Str">Never</Property>
            <Property Name="SourceItem[52].FolderTopLevelVI" Type="Str">Never</Property>
            <Property Name="SourceItem[52].ItemID" Type="Ref">/My Computer/user.lib/iLabs/ILABs_ErrorToString.vi</Property>
            <Property Name="SourceItem[52].TopLevelVI" Type="Str">Never</Property>
            <Property Name="SourceItem[53].FolderTopLevelVI" Type="Str">Never</Property>
            <Property Name="SourceItem[53].ItemID" Type="Ref">/My Computer/user.lib/iLabs/ILABs_GetVI.vi</Property>
            <Property Name="SourceItem[53].TopLevelVI" Type="Str">Never</Property>
            <Property Name="SourceItem[54].FolderTopLevelVI" Type="Str">Never</Property>
            <Property Name="SourceItem[54].ItemID" Type="Ref">/My Computer/user.lib/iLabs/ILABs_IsLoaded.vi</Property>
            <Property Name="SourceItem[54].TopLevelVI" Type="Str">Never</Property>
            <Property Name="SourceItem[55].FolderTopLevelVI" Type="Str">Never</Property>
            <Property Name="SourceItem[55].ItemID" Type="Ref">/My Computer/user.lib/iLabs/ILABs_RemotePanelHandler.vi</Property>
            <Property Name="SourceItem[55].TopLevelVI" Type="Str">Never</Property>
            <Property Name="SourceItem[56].FolderTopLevelVI" Type="Str">Never</Property>
            <Property Name="SourceItem[56].ItemID" Type="Ref">/My Computer/user.lib/iLabs/ILABs_SetBounds.vi</Property>
            <Property Name="SourceItem[56].TopLevelVI" Type="Str">Never</Property>
            <Property Name="SourceItem[57].FolderTopLevelVI" Type="Str">Never</Property>
            <Property Name="SourceItem[57].ItemID" Type="Ref">/My Computer/user.lib/iLabs/ILABs_ViStatus.vi</Property>
            <Property Name="SourceItem[57].TopLevelVI" Type="Str">Never</Property>
            <Property Name="SourceItem[58].FolderTopLevelVI" Type="Str">Never</Property>
            <Property Name="SourceItem[58].ItemID" Type="Ref">/My Computer/user.lib/iLabs/viewDataSocket.vi</Property>
            <Property Name="SourceItem[58].TopLevelVI" Type="Str">Never</Property>
            <Property Name="SourceItem[6].FolderTopLevelVI" Type="Str">Never</Property>
            <Property Name="SourceItem[6].ItemID" Type="Ref">/My Computer/internet/http/conf/mime.typ</Property>
            <Property Name="SourceItem[6].TopLevelVI" Type="Str">Never</Property>
            <Property Name="SourceItem[7].FolderTopLevelVI" Type="Str">Never</Property>
            <Property Name="SourceItem[7].ItemID" Type="Ref">/My Computer/internet/http/conf/pwd.txt</Property>
            <Property Name="SourceItem[7].TopLevelVI" Type="Str">Never</Property>
            <Property Name="SourceItem[8].FolderTopLevelVI" Type="Str">Never</Property>
            <Property Name="SourceItem[8].ItemID" Type="Ref">/My Computer/internet/http/conf/srm.cfg</Property>
            <Property Name="SourceItem[8].TopLevelVI" Type="Str">Never</Property>
            <Property Name="SourceItem[9].FolderTopLevelVI" Type="Str">Never</Property>
            <Property Name="SourceItem[9].IsFolder" Type="Bool">true</Property>
            <Property Name="SourceItem[9].ItemID" Type="Ref">/My Computer/internet/http/htdocs</Property>
            <Property Name="SourceItem[9].TopLevelVI" Type="Str">Never</Property>
            <Property Name="StripLib" Type="Bool">false</Property>
         </Item>
      </Item>
   </Item>
</Project>
