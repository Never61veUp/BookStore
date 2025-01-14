export interface Author {
    id: string;
    fullName: FullName;
    birthDate?: Date | null;
    biography?: string | null;
}
export interface FullName {
    firstName: string;
    lastName: string;
}