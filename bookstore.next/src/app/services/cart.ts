 interface Book {
     Title: string;
     AuthorFullName: string;
     Count: number;
     Price: number;
}

 interface CartResponse {
     [id: string]: Book;
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
    return await response.json();

}
export const getTotalPrice = async (): Promise<number> => {
    const response = await fetch(`http://localhost:5263/Cart/getTotalPrice`, {
        credentials: 'include',
    })
    if (!response.ok) {
        const error = await response.text();
        throw new Error(error || "Failed to get cart");
    }
    return await response.json();
}