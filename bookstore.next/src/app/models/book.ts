interface Book {
    id: string; // Guid
    title: string;
    description: string;
    image: { bookId: string, name: string, url: string };
    price: { value: number }; // Объект с числовым значением
    authorId: string; // Guid
    categoryId: string; // Guid
    stockCount: number;
    authorFullName: { firstName: string; lastName: string; middleName?: string }; // ФИО автора
    categoryName: string;
}