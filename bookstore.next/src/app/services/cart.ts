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
export const getCart = async () => {
    const response = await fetch(`http://localhost:5263/Cart/getCard`, {
        credentials: 'include',
    })
    if (!response.ok) {
        const error = await response.text();
        throw new Error(error || "Failed to get cart");
    }
    return await response.json();
}