export interface Book {
    title: string;
    authorFullName: string;
    count: number;
}

export interface CartResponse {
    bookId: string;
    books: Book;
}
export const addToCart = async (bookId: string) => {

    const response = await fetch(`http://localhost:5263/Cart/addToCard?bookId=${bookId}`, {
        method: "POST",
        credentials: 'include',
    });
    if (!response.ok) {
        const error = await response.text();
        throw new Error(error || "Failed to addBook");
    }
};
export const getCart = async (): Promise<CartResponse[]> => {
    const response = await fetch(`http://localhost:5263/Cart/getCard`, {
        credentials: 'include',
    })
    if (!response.ok) {
        const error = await response.text();
        throw new Error(error || "Failed to get cart");
    }
    const cartData = await response.json();
    return Object.entries(cartData).map(([bookId, books]) => ({
        bookId,
        books,
    })) as CartResponse[];

}