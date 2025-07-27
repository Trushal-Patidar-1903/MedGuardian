export class Common {
}


export class SearchInterface {
 has_pagination?: boolean
 sorting?: Sorting[]
 search?: string
 search_columns?: string[]
 columns?: string[]
 per_page?: number
 page?: number
}


export class Sorting {
 column: string
 order: string
}