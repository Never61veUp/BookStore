import {BookRequest} from "@/app/services/book";
import {
    Dialog,
    DialogContent,
    DialogDescription,
    DialogFooter,
    DialogHeader,
    DialogTitle,
} from "@/components/ui/dialog"
import { Input } from "@/components/ui/input"
import { Label } from "@/components/ui/label"
import {Button} from "@/components/ui/button"
import {useEffect, useState} from "react";
import { Textarea } from "@/components/ui/textarea"
import {Combobox} from "@/components/shared/combobox";
import {getAllAuthors} from "@/app/services/author";
import {getAllCategories} from "@/app/services/categories";

interface Props {
    mode: Mode;
    values: Book;
    isModalOpen: boolean;
    handleClose: () => void;
    handleCreateBook: (request: BookRequest, file: File | null) => void;
    handleUpdateBook: (id: string, request: BookRequest, file: File | null) => void;
}

export enum Mode {
    Create,
    Edit,
}

export const CreateUpdateBook = ({
                                     mode,
                                     values,
                                     isModalOpen,
                                     handleCreateBook,
                                     handleUpdateBook,
                                     handleClose
                                 }: Props) => {
    const [title, setTitle] = useState<string>("");
    const [description, setDescription] = useState<string>("");
    const [price, setPrice] = useState<number>(1);
    const [authorId, setAuthorId] = useState<string>("");
    const [categoryId, setCategoryId] = useState<string>("");
    const [stockCount, setStockCount] = useState<number>(0);
    const [authors, setAuthors] = useState<{ value: string; label: string }[]>([]);
    const [categories, setCategories] = useState<{ value: string; label: string }[]>([]);
    const [file, setFile] = useState<File | null>(null);

    useEffect(() => {
        setTitle(values.title);
        setDescription(values.description);
        setPrice(values.price.value);
        setAuthorId(values.authorId);
        setCategoryId(values.categoryId);
        setStockCount(values.stockCount);
    }, [values]);

    const getTransformedAuthors = async (): Promise<{ value: string; label: string }[]> => {
        const authorList = await getAllAuthors();
        return authorList.map(author => ({
            value: author.id,
            label: `${author.fullName.firstName} ${author.fullName.lastName}`,
        }));
    };

    const getTransformedCategories = async (): Promise<{ value: string; label: string }[]> => {
        const categoryList = await getAllCategories();
        return categoryList.map(category => ({
            value: category.id,
            label: category.name,
        }));
    };

    const handleOnOk = async () => {
        const bookRequest: BookRequest = {
            title, description, price, authorId, categoryId, stockCount
        };
        mode === Mode.Create
            ? handleCreateBook(bookRequest, file)
            : handleUpdateBook(values.id, bookRequest, file);
    };

    useEffect(() => {
        const fetchAuthors = async () => {
            const transformedAuthors = await getTransformedAuthors();
            setAuthors(transformedAuthors);
        };
        const fetchCategories = async () => {
            const transformedCategories = await getTransformedCategories();
            setCategories(transformedCategories);
        };

        fetchCategories();
        fetchAuthors();
    }, []);

    return (
        <Dialog open={isModalOpen} onOpenChange={handleClose}>
            <DialogContent className="sm:max-w-[425px]">
                <DialogHeader>
                    <DialogTitle>{mode === Mode.Create ? 'Добавить книгу' : 'Редактировать книгу'}</DialogTitle>
                    <DialogDescription>
                        Укажите информацию о книге и загрузите изображение. Нажмите "Сохранить" для подтверждения.
                    </DialogDescription>
                </DialogHeader>
                <div className="grid gap-4 py-4">
                    <div className="grid grid-cols-4 items-center gap-4">
                        <Label htmlFor="name" className="text-right">Название</Label>
                        <Input id="name" value={title} onChange={(e) => setTitle(e.target.value)} className="col-span-3" />
                    </div>
                    <div className="grid grid-cols-4 items-center gap-4">
                        <Label htmlFor="description" className="text-right">Описание</Label>
                        <Textarea id="description" value={description} onChange={(e) => setDescription(e.target.value)} className="col-span-3" />
                    </div>
                    <div className="grid grid-cols-4 items-center gap-4">
                        <Label htmlFor="price" className="text-right">Цена</Label>
                        <Input id="price" value={price} onChange={(e) => setPrice(Number(e.target.value))} className="col-span-3" />
                    </div>
                    <div className="grid grid-cols-4 items-center gap-4">
                        <Label htmlFor="author" className="text-right">Автор</Label>
                        <Combobox itemsName="Author" onSelect={(item) => setAuthorId(item.value)} items={authors} selectedItem={authors.find((author) => author.value === authorId)} />
                    </div>
                    <div className="grid grid-cols-4 items-center gap-4">
                        <Label htmlFor="category" className="text-right">Категория</Label>
                        <Combobox itemsName="Category" onSelect={(item) => setCategoryId(item.value)} items={categories} selectedItem={categories.find((category) => category.value === categoryId)} />
                    </div>
                    <div className="grid grid-cols-4 items-center gap-4">
                        <Label htmlFor="stock" className="text-right">Количество</Label>
                        <Input id="stock" value={stockCount} onChange={(e) => setStockCount(Number(e.target.value))} className="col-span-3" />
                    </div>
                    <div className="grid grid-cols-4 items-center gap-4">
                        <Label htmlFor="file" className="text-right">Изображение</Label>
                        <Input type="file" id="file" onChange={(e) => setFile(e.target.files?.[0] || null)} className="col-span-3" />
                    </div>
                </div>
                <DialogFooter>
                    <Button onClick={handleOnOk} type="submit">Сохранить изменения</Button>
                </DialogFooter>
            </DialogContent>
        </Dialog>
    );
}
