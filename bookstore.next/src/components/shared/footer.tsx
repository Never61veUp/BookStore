import { Button } from '@/components/ui/button';
import Link from 'next/link';

const Footer = () => {
    return (
        <footer className="bg-blue-900 text-white py-8 mt-16">
            <div className="max-w-7xl mx-auto flex flex-col md:flex-row justify-between items-center">
                {/* Ссылки */}
                <div className="flex flex-col md:flex-row space-y-4 md:space-y-0 md:space-x-8">
                    <div>
                        <h3 className="font-semibold text-lg">Меню</h3>
                        <ul className="space-y-2">
                            <li>
                                <Link href="/" className="hover:text-yellow-400 transition-colors">Главная</Link>
                            </li>
                            <li>
                                <Link href="/catalog" className="hover:text-yellow-400 transition-colors">Каталог</Link>
                            </li>
                            <li>
                                <Link href="/about" className="hover:text-yellow-400 transition-colors">О нас</Link>
                            </li>
                            <li>
                                <Link href="/contact" className="hover:text-yellow-400 transition-colors">Контакты</Link>
                            </li>
                        </ul>
                    </div>
                    <div>
                        <h3 className="font-semibold text-lg">Наши социальные сети</h3>
                        <ul className="space-y-2">
                            <li>
                                <Link href="#" className="hover:text-yellow-400 transition-colors">Facebook</Link>
                            </li>
                            <li>
                                <Link href="#" className="hover:text-yellow-400 transition-colors">Instagram</Link>
                            </li>
                            <li>
                                <Link href="#" className="hover:text-yellow-400 transition-colors">Twitter</Link>
                            </li>
                        </ul>
                    </div>
                </div>

                {/* Подписка на новости */}
                <div className="mt-8 md:mt-0 text-center md:text-left">
                    <h3 className="font-semibold text-lg">Подпишитесь на наши новости</h3>
                    <div className="flex items-center space-x-4 mt-4">
                        <input
                            type="email"
                            placeholder="Ваш email"
                            className="p-2 rounded-md border-2 border-gray-400 focus:outline-none focus:border-yellow-400"
                        />
                        <Button variant="outline" size="sm" className="hover:bg-yellow-600 transition-colors">
                            Подписаться
                        </Button>
                    </div>
                </div>
            </div>

            {/* Подвал */}
            <div className="mt-8 border-t border-gray-700 pt-4 text-center">
                <p className="text-sm">
                    &copy; {new Date().getFullYear()} BookStore. Все права защищены.
                </p>
            </div>
        </footer>
    );
};

export default Footer;