import 'package:firebase_auth/firebase_auth.dart';
import 'package:flutter/material.dart';
import 'package:flutter_riverpod/flutter_riverpod.dart';
import 'package:inventory/main.dart';
import 'package:inventory/pages/profile_page.dart';

enum AvatarPopupItem { profile, logout }

class CustomAppBar {
  static AppBar buildStaticAppBar(BuildContext context, User user) {
    return AppBar(
      backgroundColor: Theme.of(context).primaryColor,
      leading: AppBarLeading(user: user),
      title: Text(user.displayName!),
    );
  }
}

class AppBarLeading extends ConsumerWidget {
  const AppBarLeading({
    Key? key,
    required this.user,
  }) : super(key: key);

  final User user;

  @override
  Widget build(BuildContext context, WidgetRef ref) {
    return PopupMenuButton(
      child: Padding(
        padding: const EdgeInsets.only(left: 10, top: 8, bottom: 8),
        child: Hero(
          tag: 'googleUserPhoto',
          child: CircleAvatar(backgroundImage: NetworkImage(user.photoURL!)),
        ),
      ),
      itemBuilder: (context) => const [
        PopupMenuItem(
          child: Text('Profile'),
          value: AvatarPopupItem.profile,
        ),
        PopupMenuItem(
          child: Text('Logout'),
          value: AvatarPopupItem.logout,
        ),
      ],
      onSelected: (value) {
        if (value == AvatarPopupItem.profile) {
          Navigator.of(context).push(MaterialPageRoute(
              builder: (context) => PersonalizationPage(user: user)));
        } else if (value == AvatarPopupItem.logout) {
          final auth = ref.watch(authenticationProvider);
          auth.signOut();
        }
      },
    );
  }
}
