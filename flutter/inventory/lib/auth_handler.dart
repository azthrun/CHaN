import 'package:flutter/cupertino.dart';
import 'package:flutter_riverpod/flutter_riverpod.dart';
import 'package:inventory/main.dart';
import 'package:inventory/pages/error_page.dart';
import 'package:inventory/pages/home_page.dart';
import 'package:inventory/pages/loading_page.dart';
import 'package:inventory/pages/login_page.dart';

class AuthHandler extends ConsumerWidget {
  const AuthHandler({Key? key}) : super(key: key);

  @override
  Widget build(BuildContext context, WidgetRef ref) {
    final authState = ref.watch(authStateProvider);
    return authState.when(
      data: (data) {
        if (data != null) return HomePage(user: data);
        return const LoginPage();
      },
      error: (e, stack) => ErrorPage(exception: e, stackTrace: stack),
      loading: () => const LoadingPage(),
    );
  }
}
