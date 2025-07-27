export class DataTableColumn {
 label:string;
 field:string;
 searchField?:string;
 sortable?:boolean;
 filterable?:boolean;
 tooltip?:string;
 datatype:string;
 items?:SettingDropDownItems[];
 command?:(any?,event?) => void;
 propRenderer? : (propObj)=> any;
 propRendererHtml? : boolean;
 propCompareFields?:string;
 selectBoxData?:selectBoxData[];
 isEditOn? : boolean;
 dataFormat?: string;
 imageTitleField?: string;
 permission? : string;
 disableChangeEvent? : boolean;
 checkCondition? : (any?, event?) => any;
 isSelected? : boolean;
}


export interface SettingDropDownItems{
 label?:string;
 command:(any?,event?)=> void,
 iconClass:string,
 permission?:string,
 id?:string,
 tooltip?:string,
 sectionClass?:string,
 hasConditionShow?:boolean,
 checkCondition:(any?,event?)=> any;

}

export interface selectBoxData{
 para_id: number
 para_value: string
}
